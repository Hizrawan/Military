// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 後臺資料庫腳本執行日誌實體類
/// </summary>
public class DBLog
{
    /// <summary>
    /// 獲得/設置 主鍵ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 當前登錄名
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 獲得/設置 資料庫執行腳本
    /// </summary>
    public string SQL { get; set; } = "";

    /// <summary>
    /// 獲取/設置 執行時間
    /// </summary>
    public DateTime LogTime { get; set; }
}
