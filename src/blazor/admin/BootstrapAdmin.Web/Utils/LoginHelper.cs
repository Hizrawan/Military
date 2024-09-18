// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Utils;

/// <summary>
/// 登錄獲取默認首頁幫助類
/// </summary>
public static class LoginHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="returnUrl"></param>
    /// <param name="appId"></param>
    /// <param name="userService"></param>
    /// <param name="dictService"></param>
    /// <returns></returns>
    public static string GetDefaultUrl(BootstrapAppContext context, string? returnUrl, string? appId, IUser userService, IDict dictService)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            //1121130 這邊會找資料庫中的應用程式首頁網址, 改為當前網站位址
            // 查找 User 設置的默認應用
            //var userName = context.UserName;
            //var defaultAppId = context.AppId;
            //var schema = context.BaseUri.Scheme;
            //var host = context.BaseUri.Host;
            //appId ??= userService.GetAppIdByUserName(userName) ?? defaultAppId;

            //if (appId == defaultAppId && dictService.GetEnableDefaultApp())
            //{
            //    // 開啟默認應用
            //    appId = dictService.GetApps().FirstOrDefault(d => d.Key != defaultAppId).Key;
            //}

            //if (!string.IsNullOrEmpty(appId))
            //{
            //    var appUrl = dictService.GetHomeUrlByAppId(appId);
            //    if (!string.IsNullOrEmpty(appUrl))
            //    {
            //        returnUrl = string.Format(appUrl, schema, host).TrimEnd('/');
            //    }
            //}
            returnUrl = context.BaseUri.ToString().TrimEnd('/').Replace("/Admin/Index", string.Empty);
            if (returnUrl.IndexOf("/admin-pages/news/bulletin") < 0) returnUrl = $"{returnUrl}/admin-pages/news/bulletin"; //1130302 原指向 https://sssss/ 加上指向 Admin/EducationTraining
            //if (returnUrl.IndexOf("FrontIndex") < 0) {
            //    returnUrl = $"{returnUrl}/FrontIndex";
            //}
        }

        return returnUrl ?? "/admin-pages/news/bulletin";
    }

    /// <summary>
    /// 將字典表中的配置 1-Login-Gitee 轉化為 gitee
    /// </summary>
    /// <param name="loginTheme"></param>
    /// <returns></returns>
    public static string? GetCurrentLoginTheme(string loginTheme)
    {
        string? ret = null;
        var segs = loginTheme.Split('-');
        if (segs.Length == 3)
        {
            ret = segs[2].ToLowerInvariant();
        }
        return ret;
    }
}
