// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using System.Drawing;
using BootstrapAdmin.Library.Enums;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using System.Drawing.Printing;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class CameraUpload : IDisposable
{
    /// <summary>
    /// 必須傳入帶有主鍵Id的實體
    /// </summary>
    [Parameter]
    [NotNull]
    public object? Value { get; set; }

    /// <summary>
    /// 上傳類型：R1 報修人上傳；V1 廠商維修前上傳；V2 廠商維修後上傳；C 合約
    /// </summary>
    [Parameter]
    [NotNull]
    public string? FileType { get; set; } = "R1";

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public Func<Task>? OnCloseAsync { get; set; }

    /// <summary>
    /// 頁面的檔案類型
    /// </summary>
    [Parameter]
    [NotNull]
    public PageFileType PageFileType { get; set; } = PageFileType.Image;

    [NotNull]
    private string? TypeName { get; set; }

    [NotNull]
    private string? Id { get; set; }

    [NotNull]
    private string? FileFolder { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<CameraUpload>? Localizer { get; set; }

    private string? ImageUrl { get; set; }

    [NotNull]
    private ConsoleLogger? Logger { get; set; }

    [NotNull]
    private string? TraceOnInit { get; set; }

    [NotNull]
    private string? TraceOnError { get; set; }

    [NotNull]
    private string? TraceOnStar { get; set; }

    [NotNull]
    private string? TraceOnClose { get; set; }

    [NotNull]
    private string? TraceOnCapture { get; set; }

    private string? PlayText { get; set; }

    private string? StopText { get; set; }

    private string? PreviewText { get; set; }

    private string? SaveText { get; set; }

    private bool PlayDisabled { get; set; } = true;

    private bool StopDisabled { get; set; } = true;

    private bool CaptureDisabled { get; set; } = true;

    private List<SelectedItem> Devices { get; } = new();

    private string? DeviceId { get; set; }

    private string? DeviceLabel { get; set; }

    private string? PlaceHolderString { get; set; }

    [NotNull]
    private Camera? Camera { get; set; }

    private string? ImageContentData { get; set; }

    private string? ImageStyleString => CssBuilder.Default("width: 320px; height: 240px; border-radius: var(--bs-border-radius);")
        .AddClass("display: none;", string.IsNullOrEmpty(ImageContentData))
        .Build();
 
    //private InputText? FileDesc;
    private string? FileDesc { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IWebHostEnvironment? WebHost { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private WebClientService? WebClientService { get; set; }

    [Inject]
    [NotNull]
    private ICommon<RepairFile>? CommonService { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        if (Value == null) return;

        base.OnInitialized();

        TraceOnInit = Localizer[nameof(TraceOnInit)];
        TraceOnError = Localizer[nameof(TraceOnError)];
        TraceOnStar = Localizer[nameof(TraceOnStar)];
        TraceOnClose = Localizer[nameof(TraceOnClose)];
        TraceOnCapture = Localizer[nameof(TraceOnCapture)];
        DeviceLabel = Localizer["DeviceLabel"];
        PlaceHolderString = Localizer["PlaceHolderString"];
        PlayText = Localizer["PlayText"];
        StopText = Localizer["StopText"];
        PreviewText = Localizer["PreviewText"];
        SaveText = Localizer["SaveText"];

        //
        TypeName = Value.GetType().Name;
        Id = $"{Value.GetType().GetProperty("Id")!.GetValue(Value)}";
        FileFolder = $"upload/{TypeName}/{Id}/{FileType}/";

        //默認打開攝像頭, 可以進行拍攝
        //await OnClickOpen();
        PlayDisabled = true;
        CaptureDisabled = false;

        //OnApply(1280, 720);  // 拍攝的像素
    }

    private Task OnInit(List<DeviceItem> devices)
    {
        if (devices.Any())
        {
            Devices.AddRange(devices.Select(d => new SelectedItem(d.DeviceId, d.Label)));
            PlayDisabled = false;
        }
        else
        {
            PlaceHolderString = Localizer["PlaceHolderString"];
        }

        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task OnClickOpen()
    {
        await Camera.Open();
        PlayDisabled = true;
        StopDisabled = false;
        CaptureDisabled = false;
    }

    private async Task OnClickClose()
    {
        await Camera.Close();
        ImageContentData = null;
        PlayDisabled = false;
        StopDisabled = true;
        CaptureDisabled = true;
    }

    private async Task OnClickPreview()
    {
        ImageContentData = null;
        var stream = await Camera.Capture();
        if (stream != null)
        {
            var reader = new StreamReader(stream);
            ImageContentData = await reader.ReadToEndAsync();
            reader.Close();
        }
    }

    private Task OnClickSave() => Camera.SaveAndDownload($"capture_{DateTime.Now:hhmmss}.png");

    private Task OnApply(int width, int height) => Camera.Resize(width, height);

    private Task OnError(string err)
    {
        PlayDisabled = false;
        StopDisabled = true;
        CaptureDisabled = true;
        PlaceHolderString = Localizer["PlaceHolderString"];
        //Logger.Log($"{TraceOnError} {err}");

        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnOpen()
    {
        ImageUrl = null;
        return Task.CompletedTask;
    }

    private Task OnClose()
    {
        //Logger.Log(TraceOnClose);
        return Task.CompletedTask;
    }

    private bool SaveBase64Image(string base64String, string filePath)
    {
        var ret = true;
        try
        {
            //base64String = base64String.Trim().Replace(' ', '+');  //移除空白字符
            string hz = base64String.Split(',')[0].Split(';')[0].Split('/')[1];
            string[] str = base64String.Split(',');
            byte[] imageBytes = Convert.FromBase64String(str[1]);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                using (var image = Image.FromStream(ms))
                {
                    ////檔案夾不存在
                    if (!Directory.Exists(filePath.Substring(0, filePath.LastIndexOf("\\"))))
                    {
                        Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
                    }
                    //image.Save(filePath);  // 不加浮水印

                    using (Graphics graphic = Graphics.FromImage(image))
                    {
                        Font font = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Pixel);
                        System.Drawing.Color color = System.Drawing.Color.FromArgb(128, 255, 255, 255); // 半透明白色
                        SolidBrush brush = new SolidBrush(color);
                        //PointF point = new PointF(50f, 50f); // 水印位置
                        //RectangleF rect = new RectangleF(image.Width - 180, image.Height - 50, 180, 50);
                        PointF point = new Point(image.Width / 2, image.Height * 9 /10);

                        // 設置文字抗鋸齒
                        graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                        // 繪制水印
                        graphic.DrawString($"{Localizer["WaterMarkText"]} {DateTime.Now.ToString("yyyyMMdd HH:mm")}", font, brush, point);

                        // 保存圖片
                        image.Save(filePath);
                    }
                }
            }
        }
        catch
        {
            ret = false;
        }
        return ret;
    }

    private async Task OnSaveFile()
    {
        if (string.IsNullOrEmpty(ImageContentData))
        {
            await ToastService.Show(new ToastOption { Category = ToastCategory.Error, Title = Localizer["AddLogPGName"], Content = Localizer["PhotoRequired"] });
            return;
        }
        if (string.IsNullOrEmpty(FileDesc))
        {
            await ToastService.Show(new ToastOption { Category = ToastCategory.Error, Title = Localizer["AddLogPGName"], Content = Localizer["FileDescRequired"] });
            return;
        }

        // 保存檔案
        var OriginFileName = $"{AppContext.UserName}-Carema-{DateTime.Now.ToString("yyyyMMddHHmmss")}.png";
        var fileName = WebHost.CombineTypeNameFile(FileFolder, OriginFileName);
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        var ret = SaveBase64Image(ImageContentData, fileName);

        // 更新資料庫欄位
        if (ret)
        {
            var repariFile = new RepairFile
            {
                FileType = FileType,
                FilePath = FileFolder,
                FileName = OriginFileName,
                FileExtName = "png",
                FileDesc = FileDesc,
                DataID = int.Parse(Id)
            };
            string? PGName = Localizer["AddLogPGName"]; //功能名稱
            string? PGCode = "";    //功能代碼, 待定
            string chgType = "I";
            string loginIP = (await WebClientService.GetClientInfo()).Ip!;
            ret = await CommonService.SaveAsync(repariFile, ItemChangedTypeAction.Add, AppContext.UserName, chgType, PGName, PGCode, loginIP);
        }
        await ShowToast(ret, Localizer["FileText"]);

        //關攝像頭, 關窗
        await OnClose();

        if (OnCloseAsync != null)
        {
            await OnCloseAsync();
        }
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

    /// <summary>
    /// 釋放資源
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        Camera.Close();

        GC.SuppressFinalize(this);
    }
}
