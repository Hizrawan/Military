// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class ApLogs
{
    private List<int> PageItemsSource { get; } = new List<int> { 20, 40, 80, 100, 200 };

    private ApLogSearchModel ApLogSearchModel { get; } = new();

    [Inject]
    [NotNull]
    private IApLog? ApLogService { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<ApLogs>? Localizer { get; set; }

    private Task<QueryData<ApLog>> OnQueryAsync(QueryPageOptions options)
    {
        var ret = new QueryData<ApLog>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var filter = new ApLogFilter
        {
            ChgIP = ApLogSearchModel.ChgIP,
            Start = ApLogSearchModel.ChgTime.Start,
            End = ApLogSearchModel.ChgTime.End,
        };

        var sortList = new List<string>();
        if (options.SortOrder != SortOrder.Unset && !string.IsNullOrEmpty(options.SortName))
        {
            sortList.Add($"{options.SortName} {options.SortOrder}");
        }
        else if (options.SortList != null)
        {
            sortList.AddRange(options.SortList);
        }
        var (Items, ItemsCount) = ApLogService.GetAll(options.SearchText, filter, options.PageIndex, options.PageItems, sortList);

        ret.TotalCount = ItemsCount;
        ret.Items = Items;
        ret.IsAdvanceSearch = true;
        return Task.FromResult(ret);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="apLog"></param>
    /// <returns></returns>
    private async Task OnClickRowCallback(ApLog apLog)
    {
        var op = new DialogOption()
        {
            Title = Localizer["DetailTitle"],
            Size = Size.Large,
            Component = BootstrapDynamicComponent.CreateComponent<ApLogDetail>(
                 new Dictionary<string, object?>
                 {
                     [nameof(ApLogDetail.Value)] = apLog,
                 }
             ),

        };
        await DialogService.Show(op);
    }
}
