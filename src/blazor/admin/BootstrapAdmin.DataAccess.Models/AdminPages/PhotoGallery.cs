// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models.AdminPages;

/// <summary>
/// 
/// </summary>
[Table("FileUploaded")]
public class PhotoGallery
{
    /// <summary>
    ///
    /// </summary>
    public string? IsPubText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>

    [Required]
    [NotNull]
    public string? Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? Content { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsPub { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public bool IsDelete { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public bool IsShow { get; set; }

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
    /// <summary>
    ///
    /// </summary>
    public string? NewsType { get; set; }

}

