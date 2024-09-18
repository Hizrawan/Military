// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapBlazor.Components;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class NavigationTree
{
    [NotNull]
    private List<TreeViewItem<Navigation>>? Items { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<Navigation>? AllMenus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<string>? SelectedMenus { get; set; }

    /// <summary>
    /// 保存按鈕回檔委託
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Func<List<string>, Task<bool>>? OnSave { get; set; }

    [CascadingParameter]
    private Func<Task>? CloseDialogAsync { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<NavigationTree>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Items = AllMenus.ToTreeItemList(SelectedMenus, RenderTreeItem);
    }

    private string GetApp(string? app) => DictService.GetApps().FirstOrDefault(i => i.Key == app).Value ?? Localizer["NoSetting"];

    private async Task OnClickClose()
    {
        if (CloseDialogAsync != null)
        {
            await CloseDialogAsync();
        }
    }

    private Task OnTreeItemChecked(List<TreeViewItem<Navigation>> items)
    {
        SelectedMenus = items.SelectMany(i =>
        {
            var ret = new List<string>
            {
                i.Value.Id
            };
            if (i.Parent != null)
            {
                ret.Add(i.Parent.Value.Id);
            }
            return ret;
        }).Distinct().ToList();
        return Task.CompletedTask;
    }

    private async Task OnClickSave()
    {
        var ret = await OnSave(SelectedMenus);
        if (ret)
        {
            await OnClickClose();
            await ToastService.Success(Localizer["NavigationTreeText"], Localizer["SuccessText"]);

        }
        else
        {
            await ToastService.Error(Localizer["NavigationTreeText"], Localizer["FailedText"]);
        }
    }
}
