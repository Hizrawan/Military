// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Library.Utilities;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Services.SMS;
using BootstrapAdmin.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;

namespace BootstrapAdmin.Web.Controllers
{
    /// <summary>
    /// Account controller.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private const string MobileSchema = "Mobile";

        #region UserLogin
        /// <summary>
        /// Login the specified userName, password and remember.
        /// </summary>
        /// <returns>The login.</returns>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="verifyCode">驗證碼</param>
        /// <param name="verifyCodeAnswer">驗證碼答案，經 SHA512 雜湊函數加密</param>
        /// <param name="remember">Remember.</param>
        /// <param name="returnUrl"></param>
        /// <param name="appId"></param>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <param name="dictService"></param>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string verifyCode, string verifyCodeAnswer, [FromQuery] string? remember, [FromQuery] string? returnUrl, [FromQuery] string? appId,
            [FromServices] BootstrapAppContext context,
            [FromServices] IUser userService,
            [FromServices] IDict dictService)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(verifyCode) || string.IsNullOrEmpty(verifyCodeAnswer))
            {
                return RedirectLogin();
            }

            //登入驗證要另外判斷 IsEnable 及 IsDelete
            var auth = userService.Authenticate(userName, password, "Login") && verifyCode.ToLower().GetCipherTextBySHA512HashFunction().Equals(verifyCodeAnswer);
            var persistent = remember == "true";
            var period = 0;
            if (persistent)
            {
                // Cookie 持久化
                period = dictService.GetCookieExpiresPeriod();
            }

            context.UserName = userName;
            context.GroupId = userService.GetUserByUserName(userName)?.GroupsID;
            context.BaseUri = new Uri($"{Request.Scheme}://{Request.Host}/Admin/Index"); //1130302 原指向 https://sssss/ 加上指向 Admin/Index
            return auth ? await SignInAsync(userName, LoginHelper.GetDefaultUrl(context, returnUrl, appId, userService, dictService), persistent, period) : RedirectLogin(returnUrl);
        }

        private async Task<IActionResult> SignInAsync(string userName, string returnUrl, bool persistent, int period = 0, string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            var identity = new ClaimsIdentity(authenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));

            var properties = new AuthenticationProperties();
            if (persistent)
            {
                properties.IsPersistent = true;
            }
            if (period != 0)
            {
                properties.ExpiresUtc = DateTimeOffset.Now.AddDays(period);
            }
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

            return Redirect(returnUrl);
        }

        private IActionResult RedirectLogin(string? returnUrl = null)
        {
            var url = returnUrl;
            if (string.IsNullOrEmpty(url))
            {
                var query = Request.Query.Aggregate(new Dictionary<string, string?>(), (d, v) =>
                {
                    d.Add(v.Key, v.Value.ToString());
                    return d;
                });
                url = QueryHelpers.AddQueryString(Request.PathBase + CookieAuthenticationDefaults.LoginPath, query);
            }
            return Redirect(url);
        }
        #endregion

        #region Logout
        /// <summary>
        /// Logout this instance.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="appId"></param>
        /// <returns>The logout.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout([FromQuery] string returnUrl, [FromQuery] string appId)
        {
            await HttpContext.SignOutAsync();
            return Redirect(QueryHelpers.AddQueryString(Request.PathBase + CookieAuthenticationDefaults.LoginPath, new Dictionary<string, string?>
            {
                ["AppId"] = appId,
                ["ReturnUrl"] = returnUrl
            }));
        }
        #endregion

        #region Mobile Login
        /// <summary>
        /// 短信驗證登錄方法
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Mobile(string phone, string code, [FromQuery] string? remember, [FromQuery] string? returnUrl,
            [FromQuery] string? appId,
            [FromServices] ISMSProvider provider,
            [FromServices] IUser userService,
            [FromServices] IDict dictService,
            [FromServices] BootstrapAppContext context)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
            {
                return RedirectLogin();
            }

            var auth = provider.Validate(phone, code);
            var persistent = remember == "true";
            var period = 0;
            if (persistent)
            {
                // Cookie 持久化
                period = dictService.GetCookieExpiresPeriod();
            }
            if (auth)
            {
                userService.TryCreateUserByPhone(phone, code, context.AppId, provider.Options.Roles);
            }

            context.UserName = phone;
            context.GroupId = userService.GetUserByUserName(phone)?.GroupsID;
            context.BaseUri = new Uri(Request.Path.Value!);
            return auth ? await SignInAsync(phone, LoginHelper.GetDefaultUrl(context, returnUrl, appId, userService, dictService), persistent, period, MobileSchema) : RedirectLogin(returnUrl);
        }

        #endregion

        ///// <summary>
        ///// Accesses the denied.
        ///// </summary>
        ///// <returns>The denied.</returns>
        //[ResponseCache(Duration = 600)]
        //[HttpGet]
        //public ActionResult AccessDenied() => View("Error", ErrorModel.CreateById(403));

        ///// <summary>
        ///// Gitee 認證
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult Gitee([FromServices] IConfiguration config)
        //{
        //    var enabled = config.GetValue($"{nameof(GiteeOptions)}:Enabled", false);
        //    return Challenge(enabled ? GiteeDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        //}

        ///// <summary>
        ///// GitHub 認證
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult GitHub([FromServices] IConfiguration config)
        //{
        //    var enabled = config.GetValue($"{nameof(GitHubOptions)}:Enabled", false);
        //    return Challenge(enabled ? GitHubDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        //}

        ///// <summary>
        ///// Tencent 認證
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult Tencent([FromServices] IConfiguration config)
        //{
        //    var enabled = config.GetValue($"{nameof(TencentOptions)}:Enabled", false);
        //    return Challenge(enabled ? TencentDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        //}

        ///// <summary>
        ///// Alipay 認證
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult Alipay([FromServices] IConfiguration config)
        //{
        //    var enabled = config.GetValue($"{nameof(AlipayOptions)}:Enabled", false);
        //    return Challenge(enabled ? AlipayDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        //}

        ///// <summary>
        ///// WeChat 認證
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult WeChat([FromServices] IConfiguration config)
        //{
        //    var enabled = config.GetValue($"{nameof(WeChatOptions)}:Enabled", false);
        //    return Challenge(enabled ? WeChatDefaults.AuthenticationScheme : CookieAuthenticationDefaults.AuthenticationScheme);
        //}
        ///// <summary>
        ///// 系統鎖屏介面
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult> Lock()
        //{
        //    if (!User.Identity!.IsAuthenticated) return Login();

        //    var authenticationType = User.Identity.AuthenticationType;
        //    await HttpContext.SignOutAsync();
        //    var urlReferrer = Request.Headers["Referer"].FirstOrDefault();
        //    if (urlReferrer?.Contains("/Pages", StringComparison.OrdinalIgnoreCase) ?? false) urlReferrer = "/Pages";
        //    return View(new LockModel(User.Identity.Name)
        //    {
        //        AuthenticationType = authenticationType,
        //        ReturnUrl = WebUtility.UrlEncode(string.IsNullOrEmpty(urlReferrer) ? CookieAuthenticationDefaults.LoginPath.Value : urlReferrer)
        //    });
        //}

        ///// <summary>
        ///// 系統鎖屏介面
        ///// </summary>
        ///// <param name="provider"></param>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        ///// <param name="authType"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[IgnoreAntiforgeryToken]
        //public Task<IActionResult> Lock([FromServices] ISMSProvider provider, string userName, string password, string authType)
        //{
        //    // 根據不同的登錄方式
        //    Task<IActionResult> ret;
        //    if (authType == MobileSchema) ret = Mobile(provider, userName, password);
        //    else ret = Login(userName, password, string.Empty);
        //    return ret;
        //}
    }
}
