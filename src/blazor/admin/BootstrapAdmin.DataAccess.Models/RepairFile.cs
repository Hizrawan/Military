// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 設備實體類
/// </summary>
public class RepairFile
{
    /// <summary>
    /// 獲得/設置 主鍵 資料庫自增列
    /// </summary>
    [Key]
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 檔案類別
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// 獲得/設置 檔案名稱
    /// </summary>
    [Required]
    public string? FileName { get; set; }

    /// <summary>
    /// 獲得/設置 檔案路徑
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// 獲得/設置 檔案副檔名
    /// </summary>
    public string? FileExtName { get; set; }

    /// <summary>
    /// 獲得/設置 檔案說明
    /// </summary>
    public string? FileDesc { get; set; }

    /// <summary>
    /// 獲得/設置 資料表ID
    /// </summary>
    public int? DataID { get; set; }

    /// <summary>
    /// 獲得/設置 是否刪除
    /// </summary>
    public bool IsDelete { get; set; } = false;

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
    /// 获得/设置 刪除者
    /// </summary>
    public string? DeleteBy { get; set; }

    /// <summary>
    /// 获得/设置 刪除日期
    /// </summary>
    public DateTime? DeleteDate { get; set; }

}
