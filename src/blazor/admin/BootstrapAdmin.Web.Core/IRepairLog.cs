// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// Vendor 廠商介面
/// </summary>
public interface IRepairLog
{
    /// <summary>
    /// 取得所有廠商資料
    /// </summary>
    /// <returns></returns>
    List<RepairLog> GetAll();

}
