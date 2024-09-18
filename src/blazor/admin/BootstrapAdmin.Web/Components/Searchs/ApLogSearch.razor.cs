// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Models;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ApLogSearch
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public ApLogSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public EventCallback<ApLogSearchModel>? ValueChanged { get; set; }
}
