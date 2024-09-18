// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public class EventCarouselFilter
{
    /// <summary>
    /// 
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? IsEnable { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? FileDesc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EventCarouselStartDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? EventCarouselEndDate { get; set; }
}
