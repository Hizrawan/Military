// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class BulletinSearch
{
    [Inject]
    [NotNull]
    private IStringLocalizer<BulletinSearchModel>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<BulletinSearchModel> ValueChanged { get; set; }

    [NotNull]
    private IEnumerable<SelectedItem>? DemoValues { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public BulletinSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        DemoValues = new List<SelectedItem>(1)
        {
            new("0", Localizer["All"]),
            new("1", Localizer["正常"]),
            new("2", Localizer["過期"])
        };

        await base.OnInitializedAsync();
    }

}
