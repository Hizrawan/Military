// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 登錄服務
/// </summary>
public interface ILogin
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="IP"></param>
    /// <param name="OS"></param>
    /// <param name="browser"></param>
    /// <param name="address"></param>
    /// <param name="userAgent"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool Log(string userName, string? IP, string? OS, string? browser, string? address, string? userAgent, bool result);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageItems"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    (IEnumerable<LoginLog> Items, int ItemsCount) GetAll(string? searchText, LoginLogFilter filter, int pageIndex, int pageItems, List<string> sortList);
}
