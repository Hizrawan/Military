// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;
using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;


namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ProgramManagementSearch
{
    [Inject]
    [NotNull]
    private IStringLocalizer<ProgramManagement>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<ProgramManagementSearchModel> ValueChanged { get; set; }

    [NotNull]
    private IEnumerable<SelectedItem>? DemoValues { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public ProgramManagementSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        DemoValues = new List<SelectedItem>(1)
        {
            new("0", Localizer["All"]),
            new("1", Localizer["Applying"]),
            new("2", Localizer["Approved"]),
            new("3", Localizer["Expired"])
        };

        await base.OnInitializedAsync();
    }

}
