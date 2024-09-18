// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 廠商搜尋篩選器
/// </summary>
public class VendorFilter
{
    /// <summary>
    /// 獲得/設置 廠商代碼
    /// </summary>
    public string? VendorNo { get; set; }

    /// <summary>
    /// 獲得/設置 廠商名稱
    /// </summary>
    public string? VendorName { get; set; }


    /// <summary>
    /// 獲得/設置 廠商電話
    /// </summary>
    public string? VendorTel { get; set; }

    /// <summary>
    /// 獲得/設置 廠商傳真
    /// </summary>
    public string? VendorFax { get; set; }



    /// <summary>
    /// 獲得/設置 廠商地址
    /// </summary>
    public string? VendorAddr { get; set; }


    /// <summary>
    /// 獲得/設置 廠商聯絡人
    /// </summary>
    public string? VendorContact { get; set; }


    /// <summary>
    /// 獲得/設置 聯絡人電子郵件
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// 獲得/設置 廠商類別的字典流水號列表
    /// </summary>
    public int? VendorType { get; set; }
}
