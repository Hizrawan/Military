// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IRegisterOnline
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> SaveAsync(List<RegisterOnline> registeronline, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> SaveFrontAsync(RegisterFront registerFront, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<RegisterOnline>> GetAllAsync(RegisterOnlineFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<RegisterOnline>> GetDetailEmail(String Id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<RegisterOnline> GetDetailPrint(string category);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<RegisterOnline> GetDetail(string category);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetCategories();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool IsExist(RegisterFront registerFront);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<RegisterOnline> GetAll();



}
