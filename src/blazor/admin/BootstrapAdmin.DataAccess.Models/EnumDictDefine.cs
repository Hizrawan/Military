// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 字典定義值 0 表示系統使用，1 表示使用者自訂 默認為 1
/// </summary>
public enum EnumDictDefine
{
    /// <summary>
    /// 系統使用
    /// </summary>
    [Description]
    System,

    /// <summary>
    /// 使用者自訂
    /// </summary>
    [Description]
    Customer
}
