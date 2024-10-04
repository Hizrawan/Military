// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Bulletins
    {
        private ITableSearchModel BulletinSearchModel { get; } = new BulletinSearchModel();

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        private static IEnumerable<int> pageItemsSource => new int[]
        {
            20,
            50,
            100,
            200
        };

        [Inject]
        [NotNull]
        private IBulletin? BulletinService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        [Inject]
        [NotNull]
        private WebClientService? WebClientService { get; set; }

        [Inject]
        [NotNull]
        private ICommon<Bulletin>? CommonService { get; set; }

        [Inject]
        [NotNull]
        private IStringLocalizer<Bulletins>? Localizer { get; set; }

        private string? addModalTitle;
        private string? editModalTitle;

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            addModalTitle = Localizer["AddModalTitle"];
            editModalTitle = Localizer["EditModalTitle"];
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

            var items = await BulletinService.GetAllAsync(filter);
            foreach (var item in items)
            {
                if (item.IsPub == false)
                {
                    item.IsPubText = "草稿";
                }
                else
                {
                    item.IsPubText = "已審核";
                }
            }

            if (options.Searches.Any())
            {
                items = items.Where(options.Searches.GetFilterFunc<Bulletin>(FilterLogic.Or)).ToList();
            }

            if (options.AdvancedSortList.Count != 0)
            {
                items = items.Sort(options.AdvancedSortList).ToList();
            }

            if (options.SortName == nameof(Bulletin.CreateDate))
            {
                items = items.Sort(options.SortList).ToList();
            }

            else if (!string.IsNullOrEmpty(options.SortName))
            {
                items = items.Sort(options.SortName, options.SortOrder).ToList();
            }

            if (options.Filters.Any())
            {
                items = items.Where(options.AdvanceSearches.Concat(options.Filters).GetFilterFunc<Bulletin>()).ToList();
            }
            var total = items.Count();
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletins"></param>
        /// <returns></returns>
        private async Task<bool> OnDeleteAsync(IEnumerable<Bulletin> bulletins)
        {
            string? PGName = Localizer["DelLogPGName"];
            string? PGCode = "";
            string chgType = "D";
            string loginIP = (await WebClientService.GetClientInfo()).Ip!;
            var ret = await CommonService.DeleteAsync(bulletins, AppContext.UserName, chgType, PGName, PGCode, loginIP);

            return ret;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletin"></param>
        /// <param name="changedType"></param>
        /// <returns></returns>
        private async Task<bool> OnSaveAsync(Bulletin bulletin, ItemChangedType changedType)
        {
            if (bulletin == null) return false;

            string? PGName = changedType == ItemChangedType.Add ? Localizer["AddLogPGName"] : Localizer["EditLogPGName"];
            string? PGCode = "";
            string chgType = changedType == ItemChangedType.Add ? "I" : "U";
            string loginIP = (await WebClientService.GetClientInfo()).Ip!;

            if (bulletin.StartDate > bulletin.EndDate)
            {
                await ToastService.Show(new ToastOption()
                {
                    Title = Localizer["SaveError"],
                    Content = Localizer["DateInvalid"],
                    Category = ToastCategory.Error
                });
                return false;
            }
            else
            {
                bulletin.NewsType = "0";
                var ret = await CommonService.SaveAsync(bulletin, (BootstrapAdmin.DataAccess.Models.ItemChangedTypeAction)changedType, AppContext.UserName, chgType, PGName, PGCode, loginIP);
                return ret;
            }
        }
    }
}
