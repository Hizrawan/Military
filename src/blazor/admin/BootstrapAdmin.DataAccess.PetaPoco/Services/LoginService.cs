// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.Extensions.Localization;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class LoginService : ILogin
{
    private IDBManager DBManager { get; }

    private IStringLocalizer<LoginService> Localizer { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="localizer"></param>
    public LoginService(IDBManager db, IStringLocalizer<LoginService> localizer)
        => (DBManager, Localizer) = (db, localizer);

    public bool Log(string userName, string? IP, string? OS, string? browser, string? address, string? userAgent, bool result)
    {
        var loginUser = new LoginLog()
        {
            UserName = userName,
            LoginTime = DateTime.Now,
            Ip = IP,
            City = address,
            OS = OS,
            Browser = browser,
            UserAgent = userAgent,
            Result = result ? Localizer["Success"] : Localizer["Failed"]
        };
        using var db = DBManager.Create();
        db.Insert(loginUser);

        //更新當前登入者的最近登入時間和IP
        var ret = db.Update<User>("Set LastLoginDate = @1, LastLoginIP = @2 Where UserName = @0", userName, DateTime.Now, IP) == 1;

        return ret;
    }

    public (IEnumerable<LoginLog> Items, int ItemsCount) GetAll(string? searchText, LoginLogFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        var sql = new Sql();

        if (!string.IsNullOrEmpty(filter.IP))
        {
            sql.Where("Ip Like @0", $"%{filter.IP}%");
        }

        sql.Where("LoginTime >= @0 and LoginTime <= @1", filter.Star, filter.End);

        if (sortList.Any())
        {
            sql.OrderBy(string.Join(", ", sortList));
        }
        else
        {
            sql.OrderBy("Logintime desc");
        }

        using var db = DBManager.Create();
        var data = db.Page<LoginLog>(pageIndex, pageItems, sql);
        return (data.Items, Convert.ToInt32(data.TotalItems));
    }
}
