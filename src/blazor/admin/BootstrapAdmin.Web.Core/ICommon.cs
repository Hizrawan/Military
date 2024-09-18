// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.Reflection;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 數據庫公共操作類
/// </summary>
public interface ICommon<TModel> where TModel : class, new()
{
    /// <summary>
    /// 刪除方法 (適用于刪除一筆資料, 然后做保存刪除操作記錄; 如刪除后還有其他邏輯, 需要另寫方法)
    /// </summary>
    /// <param name="models"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(IEnumerable<TModel> models, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);


    /// <summary>
    /// 資料保存方法(同時保存操作記錄, 適用于表格具有欄位: Id, CreateBy, CreateDate, ModifyBy, ModifyDate, DeleteBy, DeleteDate)
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    Task<bool> SaveAsync(TModel model, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 對實體的特定欄位賦值
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    bool SetPropertyValue(TModel model, string propertyName, object value);

    /// <summary>
    /// 取得實體的特定欄位值
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propertyName"></param>
    object? GetPropertyValue(TModel model, string propertyName);

}
