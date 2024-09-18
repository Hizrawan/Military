// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface INavigation
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Navigation> GetAllMenus(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<string> GetMenusByRoleId(string? roleId);

    /// <summary>
    /// 根據url取得Menu名稱（首頁待辦事項看板, 點擊筆數時, 打開新tab）
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    Navigation? GetMenuByUrl(string? url);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="menuIds"></param>
    /// <returns></returns>
    bool SaveMenusByRoleId(string? roleId, List<string> menuIds);

    /// <summary>
    /// 刪除選單（可批量）
    /// </summary>
    /// <param name="menus"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    Task<bool> DeleteMenus(IEnumerable<Navigation> menus, string? userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);
}
