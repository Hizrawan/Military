// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class BulletinEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Bulletin? Value { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<BulletinEditor>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    private string FileType { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Value.CreateBy = Context.UserName;
    }
}
