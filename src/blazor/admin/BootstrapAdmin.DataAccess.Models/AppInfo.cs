// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class AppInfo
{
    /// <summary>
    /// 系統名稱
    /// </summary>
    [Required]
    [NotNull]
    public string? Title { get; set; }

    /// <summary>
    /// 網站頁腳
    /// </summary>
    [Required]
    [NotNull]
    public string? Footer { get; set; }

    /// <summary>
    /// 登錄首頁
    /// </summary>
    [NotNull]
    public string? Login { get; set; }

    /// <summary>
    /// 後臺地址
    /// </summary>
    [NotNull]
    public string? AuthUrl { get; set; }

    /// <summary>
    /// 網站主題
    /// </summary>
    [NotNull]
    public string? Theme { get; set; }

    /// <summary>
    /// 是否開啟默認應用功能
    /// </summary>
    public bool EnableDefaultApp { get; set; }

    /// <summary>
    /// 系統演示
    /// </summary>
    public bool IsDemo { get; set; }

    /// <summary>
    /// 側邊欄設置
    /// </summary>
    public bool SiderbarSetting { get; set; }

    /// <summary>
    /// 標題設置
    /// </summary>
    public bool TitleSetting { get; set; }

    /// <summary>
    /// 固定表頭
    /// </summary>
    public bool FixHeaderSetting { get; set; }

    /// <summary>
    /// 健康檢查
    /// </summary>
    public bool HealthCheckSetting { get; set; }

    /// <summary>
    /// 手機登錄
    /// </summary>
    public bool MobileLogin { get; set; }

    /// <summary>
    /// OAuth認證
    /// </summary>
    public bool OAuthLogin { get; set; }

    /// <summary>
    /// 自動鎖屏
    /// </summary>
    public bool AutoLock { get; set; }

    /// <summary>
    /// 時長間隔（秒）
    /// </summary>
    [Required]
    public int Interval { get; set; }

    /// <summary>
    /// 地理位置定位器
    /// </summary>
    public string? IpLocator { get; set; }

    /// <summary>
    /// 異常日誌（月）
    /// </summary>
    [Required]
    public int ExceptionExpired { get; set; }

    /// <summary>
    /// 操作日誌（月）
    /// </summary>
    [Required]
    public int OperateExpired { get; set; }

    /// <summary>
    /// 操作日誌（月）
    /// </summary>
    [Required]
    public int LoginExpired { get; set; }

    /// <summary>
    /// 訪問日誌（月）
    /// </summary>
    [Required]
    public int AccessExpired { get; set; }

    /// <summary>
    /// Cookie（天）
    /// </summary>
    [Required]
    public int CookieExpired { get; set; }

    /// <summary>
    /// IP 緩存（分）
    /// </summary>
    [Required]
    public int IPCacheExpired { get; set; }

    /// <summary>
    /// 授權碼
    /// </summary>
    [Required]
    [NotNull]
    public string? AuthCode { get; set; }
}
