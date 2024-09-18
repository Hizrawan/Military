// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 廠商實體類
/// </summary>
public class SendEmail
{
    /// <summary>
    /// 獲得/設置  資料庫自增列
    /// </summary>
    [Key]
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 報修ID
    /// </summary>
    [Required]
    [NotNull]
    public int DataId { get; set; } = 0;

    /// <summary>
    /// 獲得/設置 Email類別
    /// </summary>    
    [Required]
    [NotNull]
    public int EmailType { get; set; } = 0;

    /// <summary>
    /// 獲得/設置 Email狀態
    /// </summary>
    public int EmailStatus { get; set; } = 0;

    /// <summary>
    /// 獲得/設置 Email
    /// </summary>
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    /// <summary>
    /// 獲得/設置 郵件標題
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 獲得/設置 郵件內容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 獲得/設置 發送等級
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 獲得/設置 創建時間
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 獲得/設置 發送時間
    /// </summary>
    public DateTime? SendDate { get; set; }

    /// <summary>
    /// 獲得/設置 失敗原因
    /// </summary>
    public string? FailReason { get; set; }

    /// <summary>
    /// 獲得/設置 發送錯誤次數
    /// </summary>
    public int? ErrorCount { get; set; }

    // Begin 非資料庫欄位
    /// <summary>
    /// 獲得/設置 Email類別
    /// </summary>
    public string? EmailTypeValue { get; set; }

    /// <summary>
    /// 獲得/設置 Email狀態
    /// </summary>
    public string? EmailStatusValue { get; set; }

    // End 非資料庫欄位
}
