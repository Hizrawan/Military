// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 登錄使用者資訊實體類
/// </summary>
public class LoginLog
{
    /// <summary>
    /// 獲得/設置 Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 使用者名稱
    /// </summary>
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 獲得/設置 登錄時間
    /// </summary>
    public DateTime LoginTime { get; set; }

    /// <summary>
    /// 獲得/設置 登錄IP地址 主機
    /// </summary>
    [NotNull]
    public string? Ip { get; set; }

    /// <summary>
    /// 獲得/設置 登錄瀏覽器 瀏覽器
    /// </summary>
    [NotNull]
    public string? Browser { get; set; }

    /// <summary>
    /// 獲得/設置 登錄作業系統 作業系統
    /// </summary>
    [NotNull]
    public string? OS { get; set; }

    /// <summary>
    /// 獲得/設置 登錄地點
    /// </summary>
    [NotNull]
    public string? City { get; set; }

    /// <summary>
    /// 獲得/設置 登錄結果
    /// </summary>
    [NotNull]
    public string? Result { get; set; }

    /// <summary>
    /// 獲得/設置 使用者UserAgent  登錄名稱
    /// </summary>
    [NotNull]
    public string? UserAgent { get; set; }
}
