// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Library.Utilities;

using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class UserVerifyCode
{
    [Inject]
    [NotNull]
    private IStringLocalizer<UserLogin>? Localizer { get; set; }

    private string? VerifyCode { get; set; }

    private string _verifyCodeImageSrc = string.Empty;
    private string _verifyCodeAnswer = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        GetVerifyCode();
        base.OnInitialized();
    }

    private void GetVerifyCode()
    {
        var verifyCode = new VerifyCode();
        byte[] imageBytes = verifyCode.GetVerifyCodeImage();

        _verifyCodeAnswer = verifyCode.VerifyCodeText.ToLower().GetCipherTextBySHA512HashFunction();

        string base64String = Convert.ToBase64String(imageBytes);
        _verifyCodeImageSrc = $"data:image/png;base64,{base64String}";
    }
}
