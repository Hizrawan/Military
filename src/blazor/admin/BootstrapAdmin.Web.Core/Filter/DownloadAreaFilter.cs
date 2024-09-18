// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class DownloadAreaFilter
{
    /// <summary>
    /// 
    /// </summary>
    public string? FileName { get; set; }
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
    public string? FileDesc { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? DownloadAreaEndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? DownloadAreaStartDate { get; set; }
}
