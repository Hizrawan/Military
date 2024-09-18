// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// Group 實體類
/// </summary>
public class Group
{
    /// <summary>
    /// 獲得/設置 主鍵 ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 組織名稱
    /// </summary>
    [NotNull]
    [Required]
    public string? GroupName { get; set; }

    /// <summary>
    /// 獲得/設置 組織代碼
    /// </summary>
    [NotNull]
    [Required]
    public string? GroupCode { get; set; }

    /// <summary>
    /// 獲得/設置 上級組織
    /// </summary>
    public string? ParentId { get; set; } = "0";

    /// <summary>
    /// 獲得/設置 郵遞區號
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// 獲得/設置 縣市
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 獲得/設置 鄉鎮市區
    /// </summary>
    public string? Area { get; set; }

    /// <summary>
    /// 獲得/設置 地址
    /// </summary>
    public string? Addr { get; set; }

    /// <summary>
    /// 獲得/設置 組織說明
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 獲得/設置 是否啟用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 獲得/設置 組織聯絡電話
    /// </summary>
    public string? ContactTel { get; set; }

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
    /// 獲得/設置 組織類別
    /// </summary>
    public string? GroupType { get; set; }

    /// <summary>
    /// 獲得/設置 組織聯絡人
    /// </summary>
    public string? ContactPerson { get; set; }

    /// <summary>
    /// 獲得/設置 組織類別
    /// </summary>
    public string? GroupTypeName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{GroupName} ({GroupCode})";

}
