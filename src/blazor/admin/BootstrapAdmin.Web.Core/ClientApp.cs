// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public class ClientApp
{
    /// <summary>
    /// 獲得/設置 AppId 唯一標識 應用ID
    /// </summary>
    [Required]
    public string? AppId { get; set; }

    /// <summary>
    /// 獲得/設置 App 應用名稱
    /// </summary>
    [Required]
    public string? AppName { get; set; }

    /// <summary>
    /// 獲得/設置 應用首頁
    /// </summary>
    [Required]
    public string? HomeUrl { get; set; }

    /// <summary>
    /// 獲得/設置 網站標題
    /// </summary>
    [Required]
    public string? Title { get; set; }

    /// <summary>
    /// 獲得/設置 網站頁腳
    /// </summary>
    [Required]
    public string? Footer { get; set; }

    /// <summary>
    /// 獲得/設置 App Logo 網站圖示
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 獲得/設置 網站圖示
    /// </summary>
    public string? Favicon { get; set; }

    /// <summary>
    /// 個人中心地址
    /// </summary>
    public string? ProfileUrl { get; set; }

    /// <summary>
    /// 設置位址
    /// </summary>
    public string? SettingsUrl { get; set; }

    /// <summary>
    /// 消息通知位址
    /// </summary>
    public string? NotificationUrl { get; set; }
}
