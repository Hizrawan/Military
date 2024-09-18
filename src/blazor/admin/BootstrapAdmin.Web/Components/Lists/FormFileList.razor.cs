// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class FormFileList
{
    [Inject]
    [NotNull]
    private IStringLocalizer<FormFileList>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? TypeName { get; set; }

    /// <summary>
    /// 上傳類型：1 圖片; 2 文件
    /// </summary>
    [Parameter]
    public string? UploadType { get; set; } = "1";

    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter]
    public string? Value { get; set; }

    [Inject]
    [NotNull]
    private IForm? FormService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IWebHostEnvironment? WebHost { get; set; }
     
    [NotNull]
    private string FileFolder  => $"upload/{TypeName}/{Id}/";

    private List<string>? FileNames { get; set; }

    private  List<UploadFile> PreviewFileList { get; set; } = new();
     
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
         
        LoadFile(Id);
    }

    private void LoadFile(string? Id)
    {
        if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(TypeName)) return;
        if (!int.TryParse(Id, out _)) return;

        FileNames = FormService.GetFileNames(Id);

        //empty record
        if (!(FileNames?.Any() ?? false)) return;
 
        foreach (var FileName in FileNames)
        {
            var prevUrl = Path.Combine(FileFolder, FileName);  //preview url  ex: upload/Form/1  => wwwroot/upload/Form/1
            var fileNamePath = WebHost.CombineTypeNameFile(FileFolder, FileName);  
            if (File.Exists(fileNamePath))
            {
                var uploadFile = new UploadFile()
                {
                    FileName = FileName,
                    PrevUrl = prevUrl
                };
                var fi = new FileInfo(fileNamePath);
                uploadFile.Size = fi.Length;
                PreviewFileList.Add(uploadFile);
            }
        }
    }

    private async Task OnSaveFile(UploadFile file)
    {
        // 保存檔案
        var fileName = WebHost.CombineTypeNameFile(FileFolder, $"{file.OriginFileName}");
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        // 檔案大小 10 M
        var ret = await file.SaveToFileAsync(fileName, 10 * 1024 * 1000);

        // 更新資料庫欄位
        if (ret)
        {
            var prevUrl = Path.Combine(FileFolder, $"{file.OriginFileName}");  

            //這邊更新資料庫即可，不用對 PreviewFileList進行增加，因 Base.GetUploadFiles -> 取自 db中的文件集合 + 當前UploadFile
            ret = FormService.SaveFileName($"{Id}", file.OriginFileName); 

            file.PrevUrl = prevUrl;
        }
        await ShowToast(ret, Localizer["FileText"]);
    }

    private async Task<bool> OnDeleteFile(UploadFile file)
    {
        var ret = false;
        var fileName = $"{file.FileName ?? file.OriginFileName}";  //已經上傳的檔案，取FileName, 對于剛上傳的檔案, 取 OriginFileName

        if (!string.IsNullOrEmpty(fileName))
        {
            var fileNamePath = WebHost.CombineTypeNameFile(FileFolder, fileName);
            if (File.Exists(fileNamePath))
            {
                File.Delete(fileNamePath);
            }
            //如果是 PreviewFileList集合中的對象剛需要移除, 已經由base做了移除的動作, 這邊不做處理
            //var previewFile = PreviewFileList.FirstOrDefault(f => f.FileName == file.FileName);
            //if (previewFile != null)
            //    PreviewFileList.Remove(previewFile);

            ret = FormService.DeleteFileName($"{Id}", fileName);
        }
        await ShowToast(ret, Localizer["FileText"], Localizer["DeleteText"]);
        return ret;
    }

    private async Task ShowToast(bool result, string title, string? content = null)
    {
        content ??= Localizer["Upload"];

        if (result)
        {
            await ToastService.Success(title, $"{content}{title}{Localizer["SuccessText"]}");
        }
        else
        {
            await ToastService.Error(title, $"{content}{title}{Localizer["FailedText"]}");
        }
    }
}
