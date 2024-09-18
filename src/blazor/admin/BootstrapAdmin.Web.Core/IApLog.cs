// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 登錄服務
/// </summary>
public interface IApLog
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
    (IEnumerable<ApLog> Items, int ItemsCount) GetAll(string? searchText, ApLogFilter filter, int pageIndex, int pageItems, List<string> sortList);


    /// <summary>
    /// 寫操作記錄
    /// </summary>
    /// <param name="oriObject">當新增時，傳入null</param>
    /// <param name="curObject">當刪除時，傳入null</param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginUserID"></param>
    /// <param name="LoginIP"></param>
    /// <param name="columns">只檢索當前的欄位是否有更新</param>
    /// <returns></returns>
    Task SaveChangesAsync(object? oriObject, object? curObject, string? chgType, string? PGName, string? PGCode, string? LoginUserID, string? LoginIP, IEnumerable<string>? columns = null);
}
