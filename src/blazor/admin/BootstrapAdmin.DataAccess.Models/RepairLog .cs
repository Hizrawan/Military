// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 報修單日志實體類
/// </summary>
public class RepairLog
{
    /// <summary>
    /// 獲得/設置 主鍵 資料庫自增列
    /// </summary>
    [Key]
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 報修單ID
    /// </summary>
    //[Required]
    //[NotNull]
    public int? RepairId { get; set; }

    /// <summary>
    /// 獲得/設置 舊狀態
    /// </summary>
    //[Required]
    //[NotNull]
    public string? OldRepairStatus { get; set; }

    /// <summary>
    /// 獲得/設置 新狀態
    /// </summary>
    [Required]
    public string? NewRepairStatus { get; set; }

     
    /// <summary>
    /// 獲得/設置 變更者
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 獲得/設置 變更時間
    /// </summary>
    public DateTime? CreateDate { get; set; }

    // Begin 非資料庫欄位

    /// <summary>
    /// 獲得/設置 舊狀態
    /// </summary>
    public string? OldRepairStatusValue { get; set; }

    /// <summary>
    /// 獲得/設置 新狀態
    /// </summary>
    public string? NewRepairStatusValue { get; set; }

    /// <summary>
    /// 獲得/設置 變更者
    /// </summary>
    public string? CreateByValue { get; set; }

    // End 非資料庫欄位

}
