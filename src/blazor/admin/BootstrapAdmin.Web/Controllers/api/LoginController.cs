﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Library.Utilities;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Pages.Admin;
using BootstrapAdmin.Web.Services.SMS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BootstrapAdmin.Web.Controllers.Api;

/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class LoginController : ControllerBase
{
    /// <summary>
    /// 登錄認證介面
    /// </summary>
    /// <param name="user"></param>
    /// <param name="mobile"></param>
    /// <param name="userService"></param>
    /// <param name="provider"></param>
    /// <param name="Localizer"></param>
    /// <returns></returns>
    [HttpPost()]
    public AuthenticateResult Post(LoginUser user, [FromQuery] bool mobile,
        [FromServices] IUser userService,
        [FromServices] ISMSProvider provider, [FromServices] IStringLocalizer<LoginController> Localizer)
    {
        var result = new AuthenticateResult();
        if (mobile)
        {
            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
            {
                result.Authenticated = provider.Validate(user.UserName, user.Password);
            }
            if (!result.Authenticated)
            {
                result.Error = Localizer["MobileCodeError"];
            }
        }
        else
        {

            if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
            {
                result.Authenticated = userService.Authenticate(user.UserName, user.Password, "Login");
            }
            if (result.Authenticated)
            {
                if (!user.VerifyCode?.ToLower().GetCipherTextBySHA512HashFunction().Equals(user.VerifyCodeAnswer) ?? false)
                {
                    result.Authenticated = false;
                    result.Error = Localizer["VerifyCodeError"];
                }
            }
            else if (!result.Authenticated)
            {
                result.Error = Localizer["AccountPasswordError"];
            }
        }
        return result;
    }

    /// <summary>
    /// 跨域握手協議
    /// </summary>
    /// <returns></returns>
    [HttpOptions]
    public string? Options()
    {
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Authenticated { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? VerifyCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? VerifyCodeAnswer { get; set; }
    }
}
