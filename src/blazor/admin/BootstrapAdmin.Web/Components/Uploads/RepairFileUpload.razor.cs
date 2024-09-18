// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Library.Enums;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BootstrapBlazor.Components;
using System.IO;
using System.Reflection.Emit;
using System.Xml.Linq;
using System.Text;


namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class RepairFileUpload
{
    /// <summary>
    /// 必須傳入帶有主鍵Id的實體
    /// </summary>
    [Parameter]
    [NotNull]
    public object? Value { get; set; }


    //public string? UploadType { get; set; } = "1";

    /// <summary>
    /// 上傳類型：R1 報修人上傳；V1 廠商維修前上傳；V2 廠商維修後上傳；C 合約
    /// </summary>
    [Parameter]
    [NotNull]
    public string? FileType { get; set; } = string.Empty;

    /// <summary>
    /// 頁面的檔案類型
    /// </summary>
    [Parameter]
    [NotNull]
    public PageFileType PageFileType { get; set; } = PageFileType.Image;

    [Parameter]
    public bool ShowBrowseButton { get; set; } = true;
    [Parameter]
    public bool Showuploadbutton { get; set; } = true;

    [Inject]
    [NotNull]
    private IStringLocalizer<RepairFileUpload>? Localizer { get; set; }

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

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private IJSRuntime? JSRuntime { get; set; }

    [NotNull]
    private string? TypeName { get; set; }

    [NotNull]
    private string? Id { get; set; }

    [NotNull]
    private string? FileFolder { get; set; }

    private List<string>? FileNames { get; set; }

    [NotNull]
    private UpImageRepairFile? UpRepairFile { get; set; } = new UpImageRepairFile();

    [NotNull]
    private RepairFile? RepairFile { get; set; } = new UpImageRepairFile();

    private List<UploadFile> PreviewFileList { get; set; } = new();

    private long? FileSizeLimit { get; set; }

    private IJSObjectReference? module;

    private bool IsMobile { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        if (Value == null) return;

        RepairFile = PageFileType switch
        {
            PageFileType.Document => new UpDocumentRepairFile(),
            _ => new UpImageRepairFile(),
        };

        FileSizeLimit = RepairFile switch
        {
            UpDocumentRepairFile => typeof(UpDocumentRepairFile)
                .GetProperty(nameof(UpDocumentRepairFile.Document))?
                .GetCustomAttribute<FileValidationAttribute>()?
                .FileSize,
            _ => typeof(UpImageRepairFile)
                .GetProperty(nameof(UpImageRepairFile.Picture))?
                .GetCustomAttribute<FileValidationAttribute>()?
                .FileSize,
        };

        base.OnInitialized();

        TypeName = Value.GetType().Name;
        Id = $"{Value.GetType().GetProperty("Id")!.GetValue(Value)}";
        FileFolder = $"/upload/{TypeName}/{FileType}/";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isFirstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool isFirstRender)
    {
        if (isFirstRender)
        {
            module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/userAgent.js");
            IsMobile = await module.InvokeAsync<bool>("isMobile");
            if (IsMobile)
            {
                StateHasChanged();
            }
        }
    }

    private async Task OnSaveFile(UploadFile file, string fileDesc)
    {
        if (file == null) return;
        if (FileType == "0")
        {
            FileType = "Bulletins";
        }
        if (FileType == "1")
        {
            FileType = "Events";
        }
        if (FileType == "2")
        {
            FileType = "EducationTrainings";
        }
        // 保存檔案
        StringBuilder fileNameSb = new();
        // 原始檔名
        fileNameSb.Append(Path.GetFileNameWithoutExtension(file.File.Name));
        // 原始檔名和日期的分隔底線
        fileNameSb.Append('_');
        // 今日日期，以台灣時間為標準，格式為 yyyyMMdd-HHmmss
        fileNameSb.Append(DateTimeOffset.UtcNow
            .ToOffset(new(8, 0, 0))
            .ToString("yyyyMMdd-HHmmss"));
        // 副檔名
        fileNameSb.Append(Path.GetExtension(file.File.Name));

        file.FileName = fileNameSb.ToString();
        fileNameSb.Clear();

        var fileName = WebHost.CombineTypeNameFile(FileFolder, file.FileName);
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        // 檔案大小依據 FileValidation 的 FileSize 而定，預設為 5 MB
        var ret = await file.SaveToFileAsync(fileName, FileSizeLimit ?? 5 * 1024 * 1024);

        // 更新資料庫欄位
        if (ret)
        {
            var prevUrl = Path.Combine(FileFolder, file.File.Name);

            //這邊更新資料庫即可，不用對 PreviewFileList進行增加，因 Base.GetUploadFiles -> 取自 db中的文件集合 + 當前UploadFile
            //ret = BulletinService.SaveFileName($"{Id}", file.OriginFileName);
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var repariFile = new RepairFile
                    {
                        DataID = int.Parse(Id),
                        FileType = FileType,
                        FilePath = FileFolder,
                        FileName = file.FileName,
                        FileExtName = $"{file.File?.ContentType}",
                        FileDesc = fileDesc
                    };
                    string? PGName = Localizer["AddLogPGName"]; //功能名稱
                    string? PGCode = "";    //功能代碼, 待定
                    string chgType = "I";
                    string loginIP = (await WebClientService.GetClientInfo()).Ip!;
                    ret = await CommonService.SaveAsync(repariFile, ItemChangedTypeAction.Add, AppContext.UserName, chgType, PGName, PGCode, loginIP);
                    file.PrevUrl = prevUrl;
                }
                else
                {
                    var repariFile = new RepairFile
                    {
                        DataID = 0,
                        FileType = FileType,
                        FilePath = FileFolder,
                        FileName = file.FileName,
                        FileExtName = $"{file.File?.ContentType}",
                        FileDesc = fileDesc
                    };
                    string? PGName = Localizer["AddLogPGName"]; //功能名稱
                    string? PGCode = "";    //功能代碼, 待定
                    string chgType = "I";
                    string loginIP = (await WebClientService.GetClientInfo()).Ip!;
                    ret = await CommonService.SaveAsync(repariFile, ItemChangedTypeAction.Add, AppContext.UserName, chgType, PGName, PGCode, loginIP);
                    file.PrevUrl = prevUrl;
                }
            }
            catch (Exception ex)
            {
                await ShowToast(ret, ex.ToString());
            }


        }

        await ShowToast(ret, Localizer["FileText"]);
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

    private Task OnSaveRepairFile(EditContext context)
    {
        var a = Value;
        switch (RepairFile)
        {
            case UpDocumentRepairFile file:
                {
                    if (file.Document is not null)
                    {
                        var CurrentFile = new UploadFile()
                        {
                            Size = file.Document!.Size,
                            File = file.Document!,
                        };
                        var ret = OnSaveFile(CurrentFile, file.FileDesc!);

                        //刷新成空白狀態
                        file.Document = null;
                        file.FileDesc = null;
                        StateHasChanged();
                    }
                    break;
                }
            default:
                {
                    UpImageRepairFile file = (UpImageRepairFile)RepairFile;
                    if (file.Picture != null)
                    {
                        var CurrentFile = new UploadFile()
                        {
                            Size = file.Picture!.Size,
                            File = file.Picture!,
                        };
                        var ret = OnSaveFile(CurrentFile, file.FileDesc!);

                        //刷新成空白狀態
                        file.Picture = null;
                        file.FileDesc = null;
                        StateHasChanged();
                    }
                    break;
                }
        }
        return Task.CompletedTask;
    }

    private async Task OnPopViewFile()
    {
        var test = Value;
        var op = new DialogOption()
        {
            // 若要變成通用方法，不要加入個別 Model 的屬性是否較妥當？
            //Title = $"{Value.GetType().GetProperty("RepairNo")!.GetValue(Value)} - {Value.GetType().GetProperty("RepairDate")?.GetValue(Value)} ({Localizer["ViewDocument"]})",
            Title = Localizer["BrowseFileTitle"],
            Component = BootstrapDynamicComponent.CreateComponent<RepairFileList>(
               new Dictionary<string, object?>
               {
                   [nameof(RepairFileList.Id)] = int.Parse(Id),
                   [nameof(RepairFileList.FileType)] = FileType,
                   [nameof(RepairFileList.ShowDeletedButton)] = true,
               }
           ),
        };
        await DialogService.Show(op);
    }

    private async Task CaremaUpload()
    {
        var op = new DialogOption()
        {
            Title = Localizer["CaremaUploadTitle"],
            Size = Size.Large
        };
        op.Component = BootstrapDynamicComponent.CreateComponent<CameraUpload>(
               new Dictionary<string, object?>
               {
                   [nameof(CameraUpload.Value)] = Value,
                   [nameof(CameraUpload.OnCloseAsync)] = new Func<Task>(() => op.CloseDialogAsync()),
                   [nameof(CameraUpload.FileType)] = FileType,
               }
           );
        await DialogService.Show(op);
    }
}

/// <summary>
/// 圖片格式檔案上傳
/// </summary>
public class UpImageRepairFile : RepairFile
{
    /// <summary>
    /// 圖片；上限為 5 MB
    /// </summary>
    [Required]
    [FileValidation(Extensions = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" }, FileSize = 5 * 1024 * 1024)]
    public IBrowserFile? Picture { get; set; }   //IBrowserFile
}

/// <summary>
/// 文件格式檔案上傳
/// </summary>
public class UpDocumentRepairFile : RepairFile
{
    /// <summary>
    /// 檔案；上限為 15 MB
    /// </summary>
    [Required]
    [FileValidation(Extensions = new string[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".png", ".jpg", ".jpeg", ".gif", ".bmp" }, FileSize = 15 * 1024 * 1024)]
    public IBrowserFile? Document { get; set; }
}
