// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System.Text.Json;
using BootstrapAdmin.DataAccess.PetaPoco.Services;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class RepairLogList
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public string? RepairId { get; set; }
 
    [Inject]
    [NotNull]
    private IDBManager? DBManager { set; get; }

    [Inject]
    [NotNull]
    private IRepairLog? RepairLogService { set; get; }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private Task<QueryData<RepairLog>> OnQueryAsync(QueryPageOptions options)
    {
        var items = RepairLogService.GetAll().Where(i => i.RepairId == int.Parse(RepairId ?? "0")).ToList();

        // 處理模糊查詢
        if (options.Searches.Any())
        {
            items = items.Where(options.Searches.GetFilterFunc<RepairLog>(FilterLogic.Or)).ToList();
        }

        //  處理 Filter 高級搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<RepairLog>()).ToList();
        }

        return Task.FromResult(new QueryData<RepairLog>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = items
        });
    }
}

 

