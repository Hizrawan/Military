// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 合約篩選器
/// </summary>
public class ContractFilter
{
    /// <summary>
    /// 獲得/設置 合約編號
    /// </summary>
    public string? ContractNo { get; set; }

    /// <summary>
    /// 獲得/設置 合約廠商列表
    /// </summary>
    public List<int?> ContractVendorIDs { get; set; } = new();

    /// <summary>
    /// 獲得/設置 合約名稱
    /// </summary>
    public string? ContractName { get; set; }

    /// <summary>
    /// 獲得/設置 財產編號
    /// </summary>
    public string? AssetNo { get; set; }

    /// <summary>
    /// 獲得/設置 合約期間起日
    /// </summary>
    public DateTime? ContractStartDate { get; set; }

    /// <summary>
    /// 獲得/設置 合約期間迄日
    /// </summary>
    public DateTime? ContractEndDate { get; set; }
}
