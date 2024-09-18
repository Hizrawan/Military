// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 設備篩選器
/// </summary>
public class EquipmentFilter
{
    /// <summary>
    /// 獲得/設置 財產編號
    /// </summary>
    public string? AssetNo { get; set; }

    /// <summary>
    /// 獲得/設置 設備類別列表
    /// </summary>
    public List<int?> EquipmentTypeIds { get; set; } = [];

    /// <summary>
    /// 獲得/設置 設備名稱
    /// </summary>
    public string? EquipmentName { get; set; }

    /// <summary>
    /// 獲得/設置 保固起日
    /// </summary>
    public DateTime? WarrantyStartDate { get; set; }

    /// <summary>
    /// 獲得/設置 保固迄日
    /// </summary>
    public DateTime? WarrantyEndDate { get; set; }

    /// <summary>
    /// 獲得/設置 設備型號
    /// </summary>
    public string? EquipmentModel { get; set; }

    /// <summary>
    /// 獲得/設置 放置單位列表
    /// </summary>
    public List<int?> KeepDeptIds { get; set; } = [];
}
