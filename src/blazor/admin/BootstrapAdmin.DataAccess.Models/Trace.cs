// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class Trace
{
    /// <summary>
    /// 獲得/設置 主鍵ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 登錄使用者
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 獲得/設置 操作時間
    /// </summary>
    public DateTime LogTime { get; set; }

    /// <summary>
    /// 獲得/設置 登錄主機
    /// </summary>
    public string? Ip { get; set; }

    /// <summary>
    /// 獲得/設置 瀏覽器
    /// </summary>
    public string? Browser { get; set; }

    /// <summary>
    /// 獲得/設置 作業系統
    /// </summary>
    public string? OS { get; set; }

    /// <summary>
    /// 獲得/設置 操作地點
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 獲得/設置 動作頁面
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 獲得/設置 用戶主機資訊
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 獲得/設置  審閱人
    /// </summary>
    public string? Referer { get; set; }
}
