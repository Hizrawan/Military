// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 異常實體類
/// </summary>
public class Error
{
    /// <summary>
    /// 獲得/設置 主鍵
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 應用程式
    /// </summary>
    public string? AppDomainName { get; set; }

    /// <summary>
    /// 獲得/設置 請求網址
    /// </summary>
    public string? ErrorPage { get; set; }

    /// <summary>
    /// 獲得/設置 使用者 ID  使用者名
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// 獲得/設置 使用者 IP   登錄主機
    /// </summary>
    public string? UserIp { get; set; }

    /// <summary>
    /// 獲得/設置 異常類型
    /// </summary>
    public string? ExceptionType { get; set; }

    /// <summary>
    /// 獲得/設置 異常錯誤描述資訊  異常描述
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 獲得/設置 異常堆疊資訊
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// 獲得/設置 日誌時間戳記  記錄時間
    /// </summary>
    public DateTime LogTime { get; set; }

    /// <summary>
    /// 獲得/設置 分類資訊
    /// </summary>
    public string? Category { get; set; }
}
