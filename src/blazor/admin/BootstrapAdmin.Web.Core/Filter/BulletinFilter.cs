// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class BulletinFilter
{
    /// <summary>
    ///
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? EndDate { get; set; }
}
