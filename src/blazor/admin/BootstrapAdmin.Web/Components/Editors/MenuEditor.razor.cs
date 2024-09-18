// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class MenuEditor
{
    [Inject]
    [NotNull]
    private IStringLocalizer<MenuEditor>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Navigation? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? ParementMenus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? Targets { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? Apps { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    private Task OnToggleIconDialog() => DialogService.Show(new DialogOption()
    {
        Title = @Localizer["MenuIconTitle"],
        ShowFooter = false,
        Component = BootstrapDynamicComponent.CreateComponent<MenuIconList>(new Dictionary<string, object?>()
        {
            [nameof(MenuIconList.Value)] = Value.Icon,
            [nameof(MenuIconList.ValueChanged)] = EventCallback.Factory.Create<string?>(this, v => Value.Icon = v)
        })
    });

    private Task OnClearIcon()
    {
        Value.Icon = null;
        return Task.CompletedTask;
    }
}
