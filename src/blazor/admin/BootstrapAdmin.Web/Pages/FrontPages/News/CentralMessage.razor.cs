// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Pages.FrontPages.Details;
using Microsoft.Extensions.Localization;


namespace BootstrapAdmin.Web.Pages.FrontPages.News;
/// <summary>
/// 
/// </summary>
public partial class CentralMessage
{
    [Inject]
    [NotNull]
    private IStringLocalizer<FrontIndex>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IBulletin? BulletinService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    private List<Bulletin>? Bulletins { get; set; }
    private ITableSearchModel BulletinSearchModel { get; } = new BulletinSearchModel();

    private static IEnumerable<int> pageItemsSource => new int[]
    {
        10,
        20,
        40,
        80,
        100
    };
    private Task<QueryData<Bulletin>>? GoToOtherPage()
    {
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {

        Bulletins = await BulletinService.GetAllFrontAsync();
    }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    private Task ComponentOnClick(Bulletin Bulletins)
    {

        return DialogService.Show(new DialogOption()
        {
            Title = Bulletins.Title,
            BodyContext = Bulletins,
            BodyTemplate = builder =>
            {
                builder.OpenComponent<BulletinDetail>(0);
                builder.CloseComponent();
            }
        });
    }

    private async Task<QueryData<Bulletin>> OnQueryAsync(QueryPageOptions options)
    {
        BulletinFilter? filter = null;

        if (options.SearchModel is BulletinSearchModel searchModel)
        {
            if (searchModel.startDate > searchModel.endDate)
            {
                await ToastService.Show(new ToastOption()
                {
                    Title = Localizer["SearchError"],
                    Content = Localizer["SearchDateInvalid"],
                    Category = ToastCategory.Error
                });
                return new QueryData<Bulletin>()
                {

                    IsFiltered = options.Filters.Count > 0,
                    IsSearch = options.CustomerSearches.Count > 0 || !string.IsNullOrWhiteSpace(options.SearchText),
                    IsAdvanceSearch = options.CustomerSearches.Count > 0 && string.IsNullOrWhiteSpace(options.SearchText),
                    IsSorted = true,
                };
            }
            else
            {
                filter = new()
                {
                    Title = searchModel.Title,
                    Content = searchModel.Content,
                    State = searchModel.State,
                    StartDate = searchModel.startDate,
                    EndDate = searchModel.endDate,
                };
            }
        }

        var items = await BulletinService.GetAllFrontAsync(filter);


        if (options.Searches.Any())
        {
            items = items.Where(options.Searches.GetFilterFunc<Bulletin>(FilterLogic.Or)).ToList();
        }


        if (options.Filters.Any())
        {
            items = items.Where(options.AdvanceSearches.Concat(options.Filters).GetFilterFunc<Bulletin>()).ToList();
        }
        var total = items.Count;
        items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();
        return new QueryData<Bulletin>()
        {
            TotalCount = total,
            IsFiltered = options.Filters.Count > 0,
            IsSearch = options.CustomerSearches.Count > 0 || !string.IsNullOrWhiteSpace(options.SearchText),
            IsAdvanceSearch = options.CustomerSearches.Count > 0 && string.IsNullOrWhiteSpace(options.SearchText),
            IsSorted = true,
            Items = items
        };
    }


}
