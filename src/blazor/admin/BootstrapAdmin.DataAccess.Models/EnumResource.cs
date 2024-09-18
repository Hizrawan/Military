// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 資源類型枚舉 0 表示選單 1 表示資源 2 表示按鈕
/// </summary>
public enum EnumResource
{
    /// <summary>
    /// 選單
    /// </summary>
    [Description]
    Navigation,

    /// <summary>
    /// 資源
    /// </summary>
    [Description]
    Resource,

    /// <summary>
    /// 代碼塊
    /// </summary>
    [Description]
    Block
}
