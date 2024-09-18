// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Services;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ParentGroupTree
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public string? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [NotNull]
    private List<TreeViewItem<Group>>? Items { get; set; }

    [Inject]
    [NotNull]
    private IGroup? GroupService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        var items = GroupService.GetAll()
            .ToList();     //.Where(g => g.ParentId == "0")
        Items = items.ToGroupTreeItemList([Value], RenderTreeItem);

        await base.OnInitializedAsync();
    }

    private async Task OnTreeItemClick(TreeViewItem<Group> item)
    {
        Value = item.Value.Id;
        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Value);
        }
    }

    //private string GetApp(string? app) => DictService.GetApps().FirstOrDefault(i => i.Key == app).Value ?? "未設置";
}
