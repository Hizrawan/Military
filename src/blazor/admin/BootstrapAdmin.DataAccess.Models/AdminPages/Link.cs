// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models.AdminPages;

/// <summary>
/// 
/// </summary>
[Table("Links")]
public class Link
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? WebName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? WebLink { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? UnitId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsEnable { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsDelete { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? CreateBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? CreateDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ModifyBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? ModifyDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? DeleteBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? DeleteDate { get; set; }

}

