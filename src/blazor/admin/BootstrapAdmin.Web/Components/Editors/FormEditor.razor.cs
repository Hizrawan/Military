// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using Microsoft.Extensions.Localization;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class FormEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Form? Value { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<FormEditor>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        //初始化建立人訊息
        Value.CreateBy = Context.UserName;
    }
}
