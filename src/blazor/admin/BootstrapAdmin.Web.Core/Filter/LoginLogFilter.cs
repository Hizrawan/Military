// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public class LoginLogFilter
{
    /// <summary>
    /// 
    /// </summary>
    public DateTime Star { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime End { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? IP { get; set; }
}
