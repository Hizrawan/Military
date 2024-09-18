// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class BulletinFile
{
    /// <summary>
    /// 
    /// </summary>
    [Key]
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? FileType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string? FileName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? FileExtName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string? FileDesc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? DataID { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsDelete { get; set; } = false;

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
