// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ProgramManagementEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public ProgramManagement? Value { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<ProgramManagementEditor>? Localizer { get; set; }

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
