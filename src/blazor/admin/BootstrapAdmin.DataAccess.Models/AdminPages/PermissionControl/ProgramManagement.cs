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
[Table("Program")]
public class ProgramManagement
{
    public string? Id { get; set; }
    public string? ProgramName { get; set; }
    public string? ProgramAlias { get; set; }
    public string? ProgramPath { get; set; }
    public string? ImagePath { get; set; }
    public string? StartProgram { get; set; }
    public string? Sort { get; set; }
    public string? Category { get; set; }
    public string? QualityCode { get; set; }
    public string? LongName { get; set; }
    public string? Hide { get; set; }

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

