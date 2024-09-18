// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class RepairFileList
{
    /// <summary>
    /// Repairs.Id
    /// </summary>
    [Parameter]
    [NotNull]
    public int Id { get; set; }

    /// <summary>
    /// 上傳類型：R1 報修人上傳；V1 廠商維修前上傳；V2 廠商維修後上傳；C 合約
    /// </summary>
    [Parameter]
    [NotNull]
    public string? FileType { get; set; } = "NB";

    /// <summary>
    /// 是否可刪除檔案
    /// </summary>
    [Parameter]
    public bool ShowDeletedButton { get; set; } = true;

    [Inject]
    [NotNull]
    private IStringLocalizer<RepairFileList>? Localizer { get; set; }


    [Inject]
    [NotNull]
    private IRepairFile? RepairFileService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IWebHostEnvironment? WebHost { get; set; }

    [Inject]
    [NotNull]
    private DownloadService? DownloadService { get; set; }

    [Inject]
    [NotNull]
    private ICommon<RepairFile>? CommonService { get; set; }

    [Inject]
    [NotNull]
    private WebClientService? WebClientService { get; set; }

    private List<UploadFile> PreviewFileList { get; set; } = new();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        LoadFile(Id, FileType);
    }

    private void LoadFile(int Id, string fileType = "")
    {
        if (Id == 0 || string.IsNullOrEmpty(fileType)) return;

        var repairFiles = RepairFileService.GetByDataId(Id, fileType);
        foreach (var repairFile in repairFiles)
        {
            var fileName = repairFile.FileName!;
            var fileFolder = repairFile.FilePath!;
            var prevUrl = Path.Combine(fileFolder, fileName);  //preview url  ex: upload/RepairFile/1  => wwwroot/upload/RepairFile/1
            var fileNamePath = WebHost.CombineTypeNameFile(fileFolder, fileName);
            if (File.Exists(fileNamePath))
            {
                var uploadFile = new UploadFile()
                {
                    FileName = fileName,
                    PrevUrl = prevUrl
                };
                var fi = new FileInfo(fileNamePath);
                uploadFile.Size = fi.Length;
                PreviewFileList.Add(uploadFile);
            }
        }
    }

    private async Task<bool> OnDeleteFile(UploadFile file)
    {
        var ret = false;
        var fileName = $"{file.FileName ?? file.OriginFileName}";  //已經上傳的檔案，取FileName, 對于剛上傳的檔案, 取 OriginFileName

        if (!string.IsNullOrEmpty(fileName))
        {
            var fileNamePath = WebHost.CombineTypeNameFile(file.PrevUrl!); // PrevUrl 已經包含fileName
            if (File.Exists(fileNamePath))
            {
                File.Delete(fileNamePath);
            }

            var repairFile = RepairFileService.GetByDataIdAndFileName(Id, fileName, FileType);
            if (repairFile != null)
            {
                string? PGName = Localizer["DelLogPGName"]; //功能名稱
                string? PGCode = "";    //功能代碼, 待定
                string chgType = "D";
                string loginIP = (await WebClientService.GetClientInfo()).Ip!;

                // 這邊不做實際刪除
                // 改成調用公共方法
                ret = await CommonService.DeleteAsync(repairFile, AppContext.UserName, chgType, PGName, PGCode, loginIP);
            }
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

    private async Task OnDownload(UploadFile file)
    {
        try
        {
            var fileName = $"{file.FileName ?? file.OriginFileName}";
            var filePath = WebHost.CombineTypeNameFile(file.PrevUrl!);
            await using var stream = File.OpenRead(filePath);
            await DownloadService.DownloadFromStreamAsync(fileName, stream);
        }
        catch (FileNotFoundException msg)
        {
            await ToastService.Error(Localizer["DownLoad"], msg.Message);
        }
    }
}
