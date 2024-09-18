﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Utils;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class MenusSearch
{
    [NotNull]
    [Inject]
    private IDict? DictService { get; set; }

    private IEnumerable<SelectedItem>? CategoryItems { get; set; }

    private IEnumerable<SelectedItem>? ResourceItems { get; set; }

    private List<SelectedItem>? AppItems { get; set; }

    private List<SelectedItem>? TargetItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public MenusSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<MenusSearchModel> ValueChanged { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<MenusSearch>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        TargetItems = new List<SelectedItem>()
        {
            new SelectedItem("", Localizer["AllText"])
        };
        TargetItems.AddRange(LookupHelper.GetTargets());

        AppItems = new List<SelectedItem>()
        {
            new SelectedItem("", Localizer["AllText"])
        };
        AppItems.AddRange(DictService.GetApps().ToSelectedItemList());

        ResourceItems = typeof(EnumResource).ToSelectList(new SelectedItem("", Localizer["AllText"]));
        CategoryItems = typeof(EnumNavigationCategory).ToSelectList(new SelectedItem("", Localizer["AllText"]));
    }
}
