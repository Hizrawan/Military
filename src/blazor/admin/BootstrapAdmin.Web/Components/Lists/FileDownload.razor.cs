// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;
using BootstrapBlazor.Components;
using System.IO;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 共用：檔案下載
/// </summary>
public partial class FileDownload<TValue>
{
    /// <summary>
    /// 获得/设置 檔案列表
    /// </summary>
    [Parameter]
    [NotNull]
    public List<UploadFile> DownLoadFileList { get; set; } = new();

    /// <summary>
    /// 获得/设置 下载按钮图标
    /// </summary>
    [Parameter]
    public string? DownloadIcon { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<FileDownload<TValue>>? Localizer { get; set; }
     
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
    private IIconTheme? IconTheme { get; set; }

    private string? DownloadIconString => CssBuilder.Default("download-icon")
        .AddClass(DownloadIcon)
        .Build();
     
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

    /// <summary>
    /// 下載檔案
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    protected async Task OnDownloadAsync(UploadFile file)
    {
        try
        {
            var fileName = $"{file.FileName ?? file.OriginFileName}";
            var filePath = WebHost.CombineTypeNameFile(file.PrevUrl!); // PrevUrl 已經包含fileName, 
            await using var stream = File.OpenRead(filePath);
            await DownloadService.DownloadFromStreamAsync(fileName, stream);
        }
        catch (FileNotFoundException msg)
        {
            await ToastService.Error(Localizer["DownLoad"], msg.Message);
        }
    }

    /// <summary>
    /// OnParametersSet 方法
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        DownloadIcon ??= IconTheme.GetIconByKey(ComponentIcons.ButtonUploadDownloadIcon);
    }
}
