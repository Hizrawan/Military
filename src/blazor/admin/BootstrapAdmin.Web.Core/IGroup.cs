// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IGroup
{
    /// <summary>
    /// 獲得所有使用者
    /// </summary>
    /// <returns></returns>
    List<Group> GetAll();


    /// <summary>
    /// 刪除使用者（可批次）
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    Task<bool> DeleteGroups(IEnumerable<Group> groups, string? userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    List<string> GetGroupsByUserId(string? userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<string> GetGroupsByRoleId(string? roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds);

    /// <summary>
    /// 判斷是否有子節點
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public bool HasChildren(string? groupId);
}
