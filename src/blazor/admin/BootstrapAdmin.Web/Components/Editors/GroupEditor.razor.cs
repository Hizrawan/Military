// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class GroupEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Group? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<Address>? Addresses { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? ParementGroups { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<GroupEditor>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Group>? ModelLocalizer { get; set; }

    private List<SelectedItem>? GroupTypeSelect { get; set; }

    private List<SelectedItem>? CountySelect { get; set; }

    private List<SelectedItem>? TownshipSelect { get; set; }

    private List<SelectedItem>? VillageSelect { get; set; }

    /// <summary>
    /// [覆寫方法] 元件初始化
    /// </summary>
    protected override void OnInitialized()
    {
        CountySelect = Addresses.GroupBy(adr => adr.CountyName)
            .Where(gro => !string.IsNullOrWhiteSpace(gro.Key))
            .Select(gro => new SelectedItem(gro.Key, gro.Key)
            {
                Active = gro.Key?.Equals(Value.City) ?? false
            })
            .ToList();
        CountySelect.Insert(0, new(string.Empty, Localizer["CountyItemNoSelect"]));

        if (Addresses.DistinctBy(adr => adr.CountyName).Any(adr => adr.CountyName == Value.City))
        {
            TownshipSelect = Addresses.Where(adr => adr.CountyName == Value.City)
                .GroupBy(adr => adr.TownshipName)
                .Where(gro => !string.IsNullOrWhiteSpace(gro.Key))
                .Select(gro => new SelectedItem(gro.Key!, gro.Key!)
                {
                    Active = gro.Key?.Equals(Value.Area) ?? false
                })
                .ToList();
            TownshipSelect.Insert(0, new(string.Empty, Localizer["TownshipItemNoSelect"]));
        }
        base.OnInitialized();
    }

    /// <summary>
    /// [覆寫方法] [非同步] 元件初始化
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        GroupTypeSelect = (await DictService.GetKeyValueByCategoryAsync(nameof(Group.GroupType)))
            .Select(kvp => new SelectedItem(kvp.Value.Code, kvp.Key))
            .ToList();
        await base.OnInitializedAsync();
    }

    private IEnumerable<SelectedItem> OnParementGroupsSearchTextChanged(string searchText)
    {
        return ParementGroups.Where(i => i.Text!.Contains(searchText, StringComparison.OrdinalIgnoreCase)).Select(i => new SelectedItem(i.Value!, i.Text!));
    }

    private async Task OnCountySelectedChanged(SelectedItem item)
    {
        await Task.Delay(10);
        if (string.IsNullOrWhiteSpace(item.Value))
        {
            Value.ZipCode = string.Empty;
            TownshipSelect = [];
            VillageSelect = [];
        }
        else
        {
            TownshipSelect = Addresses.Where(adr => adr.CountyName == item.Value)
                .GroupBy(adr => adr.TownshipName)
                .Where(gro => !string.IsNullOrWhiteSpace(gro.Key))
                .Select(gro => new SelectedItem(gro.Key!, gro.Key!))
                .ToList();
            TownshipSelect.Insert(0, new(string.Empty, Localizer["TownshipItemNoSelect"]));
            VillageSelect = [];
        }
        StateHasChanged();
    }

    private async Task OnTownshipSelectedChanged(SelectedItem item)
    {
        await Task.Delay(10);
        if (string.IsNullOrWhiteSpace(item.Value))
        {
            Value.ZipCode = string.Empty;
            VillageSelect = [];
        }
        else
        {
            Value.ZipCode = Addresses.FirstOrDefault(adr => adr.TownshipName == item.Value)?.ZipId;
            VillageSelect = Addresses.Where(adr => adr.TownshipName == item.Value)
                .GroupBy(adr => adr.VillageName)
                .Where(gro => !string.IsNullOrWhiteSpace(gro.Key))
                .Select(gro => new SelectedItem(gro.Key!, gro.Key!))
                .ToList();
            VillageSelect.Insert(0, new(string.Empty, Localizer["VillageItemNoSelect"]));
        }
        StateHasChanged();
    }
}
