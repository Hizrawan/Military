// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class DictEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Dict? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsEnable { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<DictEditor>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }
}
