// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class LeaveMessageFilter
{
    /// <summary>
    /// 
    /// </summary>
    public string? MessageTitle { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Sender { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ContactNumber { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? IsProcessed { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? State { get; set; }


    /// <summary>
    ///
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? LeaveMessageEndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? LeaveMessageStartDate { get; set; }
}
