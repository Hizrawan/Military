// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 功能表分類 0 表示系統功能表 1 表示使用者自訂功能表
/// </summary>
public enum EnumNavigationCategory
{
    /// <summary>
    /// 後臺選單
    /// </summary>
    [Description]
    System,

    /// <summary>
    /// 前臺選單
    /// </summary>
    [Description]
    Customer
}
