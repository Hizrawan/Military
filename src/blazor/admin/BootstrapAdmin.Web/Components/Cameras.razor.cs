// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapBlazor.Components;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class Cameras 
{
    [Inject]
    [NotNull]
    private IStringLocalizer<Cameras>? Localizer { get; set; }

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

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
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

        //foreach (var item in devices)
        //{
        //    Logger.Log($"{TraceOnInit} {item.Label}-{item.DeviceId}");
        //}

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
        //Logger.Log(TraceOnStar);
        return Task.CompletedTask;
    }

    private Task OnClose()
    {
        //Logger.Log(TraceOnClose);
        return Task.CompletedTask;
    }

}
