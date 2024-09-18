// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;

/// <summary>
/// 
/// </summary>
[Table("Unit")]
public class UnitMaintenance
{
    public string? Id { get; set; }
    public string? UnitName { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Code { get; set; }
    public string? UnitSupervisor { get; set; }
    public string? OldCode { get; set; }
    public string? StartIP { get; set; }
    public string? EndIP { get; set; }
    public string? IP { get; set; }
    public string? BusinessContent { get; set; }
    public string? Allocation { get; set; }
    public string? UnitOrder { get; set; }
    public string? UnitSort { get; set; }
    public string? IsEnable { get; set; }
    public string? OtherInfo { get; set; }
    public string? Ext { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDelete { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DeleteBy { get; set; }
}

