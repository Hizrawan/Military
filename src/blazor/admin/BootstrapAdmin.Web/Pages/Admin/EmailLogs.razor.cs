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
public partial class EmailLogs
{
    private List<int> PageItemsSource { get; } = new List<int> { 20, 40, 80, 100, 200 };

    private EmailLogSearchModel EmailLogSearchModel { get; } = new();

    [Inject]
    [NotNull]
    private ISendEmail? SendEmailService { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<SendEmail>? Localizer { get; set; }

    private Task<QueryData<SendEmail>> OnQueryAsync(QueryPageOptions options)
    {
        var items = SendEmailService.GetAll();

        // 處理模糊查詢
        if (options.Searches.Any())
        {
            items = items.Where(options.Searches.GetFilterFunc<SendEmail>(FilterLogic.Or)).ToList();
        }

        //  處理 Filter 高級搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<SendEmail>()).ToList();
        }

        return Task.FromResult(new QueryData<SendEmail>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = items
        });

    }
}
