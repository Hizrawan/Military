// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class LinkFilter
{
    /// <summary>
    ///
    /// </summary>
    public string? State { get; set; }

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
    public DateTime? LinkEndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? LinkStartDate { get; set; }
}
