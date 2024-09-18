// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 地址實體類
/// </summary>
public class Address
{
    /// <summary>
    /// 獲得/設置 地址主鍵 資料庫自增列
    /// </summary>
    [Key]
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 縣市編號
    /// </summary>
    [NotNull]
    [Required]
    public string? CountyId { get; set; }

    /// <summary>
    /// 獲得/設置 縣市名稱
    /// </summary>
    [NotNull]
    [Required]
    public string? CountyName { get; set; }

    /// <summary>
    /// 獲得/設置 鄉鎮市區編號
    /// </summary>
    public string? TownshipId { get; set; }

    /// <summary>
    /// 獲得/設置 鄉鎮市區名稱
    /// </summary>
    public string? TownshipName { get; set; }

    /// <summary>
    /// 獲得/設置 村里編號
    /// </summary>
    public string? VillageId { get; set; }

    /// <summary>
    /// 獲得/設置 村里名稱
    /// </summary>
    public string? VillageName { get; set; }

    /// <summary>
    /// 獲得/設置 郵遞區號
    /// </summary>
    public string? ZipId { get; set; }

    /// <summary>
    /// 獲得/設置 排序
    /// </summary>
    public int? ShowOrder { get; set; }
}
