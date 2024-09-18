// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// Bootstrap Admin 後臺管理功能表相關操作實體類
/// </summary>
public class Navigation
{
    /// <summary>
    /// 獲得/設置 選單主鍵ID
    /// </summary>
    [NotNull]
    [Key]
    public string? Id { set; get; }

    /// <summary>
    /// 獲得/設置 父級選單ID 預設為 0
    /// </summary>
    public string ParentId { set; get; } = "0";

    /// <summary>
    /// 獲得/設置 選單名稱
    /// </summary>
    [Required]    
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 獲得/設置 選單序號
    /// </summary>
    [Required]
    public int Order { get; set; }

    /// <summary>
    /// 獲得/設置 功能表圖示
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 獲得/設置 選單URL位址
    /// </summary>
    [NotNull]
    public string? Url { get; set; }

    /// <summary>
    /// 獲得/設置 功能表分類, 0 表示系統功能表 1 表示使用者自訂功能表
    /// </summary>
    public EnumNavigationCategory Category { get; set; }

    /// <summary>
    /// 獲得/設置 連結目標
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// 獲得/設置 是否為資源檔, 0 表示選單 1 表示資源 2 表示按鈕
    /// </summary>
    public EnumResource IsResource { get; set; }

    /// <summary>
    /// 獲得/設置 所屬應用程式，此屬性由BA後臺字典表分配
    /// </summary>
    public string? Application { get; set; }

    /// <summary>
    /// 獲得/設置 排序
    /// </summary>    
    [Required]
    public int? AppSort { get; set; }

    /// <summary>
    /// 獲得/設置 是否啟用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 獲得/設置 是否刪除
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 獲得/設置 建立者
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 獲得/設置 建立日
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// 獲得/設置 最後修改者
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 獲得/設置 最後修改時間
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 獲得/設置，是否有子選單
    /// </summary>
    public bool HasChildren { get; set; }
}
