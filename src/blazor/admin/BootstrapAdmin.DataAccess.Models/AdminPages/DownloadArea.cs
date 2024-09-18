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
public class DownloadArea
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DataId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? FileType { get; set; }
    /// <summary>
    /// 
    /// </summary>
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
    public string? FileDesc { get; set; }
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

