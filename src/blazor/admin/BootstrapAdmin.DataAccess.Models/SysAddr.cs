// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
[Table("SysAddrs")]
public class SysAddr
{
    /// <summary>
    /// 獲得/設置 字典主鍵 資料庫自增列
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 行政代碼
    /// </summary>
    [Required]
    public string? ZAP_ALL { get; set; }

    /// <summary>
    /// 獲得/設置 縣市
    /// </summary>
    [Required]
    [NotNull]
    public string? PRO_ID { get; set; }

    /// <summary>
    /// 獲得/設置 縣市名稱
    /// </summary>
    [Required]
    [NotNull]
    public string? PRO_DESC { get; set; }

    /// <summary>
    /// 獲得/設置 公所
    /// </summary>
    [Required]
    [NotNull]
    public string? COUNTY_ID { get; set; }

    /// <summary>
    /// 獲得/設置 公所名稱
    /// </summary>
    [Required]
    [NotNull]
    public string? COUNTY_DESC { get; set; }

    /// <summary>
    /// 獲得/設置 村里別
    /// </summary>
    [Required]
    [NotNull]
    public string? VILLAGE_ID { get; set; }

    /// <summary>
    /// 獲得/設置 村里別
    /// </summary>
    [Required]
    [NotNull]
    public string? VILLAGE_DESC { get; set; }

    /// <summary>
    /// 獲得/設置 郵遞編碼
    /// </summary>
    public string? ZIP_ID { get; set; }

    /// <summary>
    /// 獲得/設置 排序
    /// </summary>
    public string? ORDER_ID { get; set; }
}
