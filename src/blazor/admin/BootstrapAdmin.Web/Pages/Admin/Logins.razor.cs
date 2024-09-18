// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Logins
{
    private List<int> PageItemsSource { get; } = new List<int> { 20, 40, 80, 100, 200 };

    private LoginLogSearchModel LoginLogSearchModel { get; } = new();

    [Inject]
    [NotNull]
    private ILogin? LoginLogService { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Logins>? Localizer { get; set; }

    private Task<QueryData<LoginLog>> OnQueryAsync(QueryPageOptions options)
    {
        var ret = new QueryData<LoginLog>()
        {
            IsSorted = true,
            IsFiltered = true,
            IsSearch = true
        };

        var filter = new LoginLogFilter
        {
            IP = LoginLogSearchModel.IP,
            Star = LoginLogSearchModel.LogTime.Start,
            End = LoginLogSearchModel.LogTime.End,
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
        var (Items, ItemsCount) = LoginLogService.GetAll(options.SearchText, filter, options.PageIndex, options.PageItems, sortList);

        ret.TotalCount = ItemsCount;
        ret.Items = Items;
        ret.IsAdvanceSearch = true;
        return Task.FromResult(ret);
    }
}
