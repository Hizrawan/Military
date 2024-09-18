﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Services.SMS;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class SMSLogin : IDisposable
{
    private bool IsSendCode { get; set; } = true;

    private string? SendCodeText { get; set; } 

    private CancellationTokenSource? CancelToken { get; set; }

    [NotNull]
    private string? PhoneNumber { get; set; }

    private string? Code { get; set; }

    [Inject]
    [NotNull]
    private ISMSProvider? SMSProvider { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<SMSLogin>? Localizer { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        SendCodeText = Localizer["SendCodeText"];
    }

        async Task OnSendCode()
    {
        if (!string.IsNullOrEmpty(PhoneNumber))
        {
            var result = await SMSProvider.SendCodeAsync(PhoneNumber);
            if (result.Result)
            {
#if DEBUG
                Code = result.Data;
#endif
                IsSendCode = false;
                var count = 60;
                CancelToken ??= new CancellationTokenSource();
                while (CancelToken != null && !CancelToken.IsCancellationRequested && count > 0)
                {
                    SendCodeText = $"{Localizer["SendCodeText"]} ({count--})";
                    StateHasChanged();
                    await Task.Delay(1000, CancelToken.Token);
                }
                SendCodeText = Localizer["SendCodeText"];
            }
            else
            {
                // 短信發送失敗
            }
        }
        else
        {
            // 手機號不可為空
        }
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (CancelToken != null)
            {
                CancelToken.Cancel();
                CancelToken.Dispose();
                CancelToken = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
