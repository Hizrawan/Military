// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;
using System.Data;

namespace BootstrapAdmin.Web.Pages.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dicts
    {
        [Inject]
        [NotNull]
        private IDict? DictService { get; set; }

        [Inject]
        [NotNull]
        private BootstrapAppContext? AppContext { get; set; }

        private ITableSearchModel DictsSearchModel { get; } = new DictsSearchModel();

        [Inject]
        [NotNull]
        private IStringLocalizer<Dicts>? Localizer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        [NotNull]
        private WebClientService? WebClientService { get; set; }

        [Inject]
        [NotNull]
        private ICommon<Dict>? CommonService { get; set; }

        private string? AddModalTitle;
        private string? EditModalTitle;

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            AddModalTitle = Localizer["AddModalTitle"];
            EditModalTitle = Localizer["EditModalTitle"];

            base.OnInitialized();
        }

        private Task<QueryData<Dict>> OnQueryAsync(QueryPageOptions options)
        {
            var items = DictService.GetHeaderAll();

            // 處理模糊查詢
            if (options.Searches.Any())
            {
                items = items.Where(options.Searches.GetFilterFunc<Dict>(FilterLogic.Or)).ToList();
            }

            //  處理 Filter 高級搜索
            if (options.CustomerSearches.Any() || options.Filters.Any())
            {
                items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<Dict>()).ToList();
            }

            return Task.FromResult(new QueryData<Dict>()
            {
                IsFiltered = true,
                IsSearch = true,
                IsSorted = true,
                Items = items
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private Task<QueryData<Dict>> OnDetailQueryAsync(QueryPageOptions options, Dict dict)
        {
            var items = DictService.GetDetail(dict.Category ?? string.Empty);

            // 處理模糊查詢
            if (options.Searches.Any())
            {
                items = items.Where(options.Searches.GetFilterFunc<Dict>(FilterLogic.Or)).ToList();
            }

            //  處理 Filter 高級搜索
            if (options.CustomerSearches.Any() || options.Filters.Any())
            {
                items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<Dict>()).ToList();
            }

            return Task.FromResult(new QueryData<Dict>()
            {
                IsFiltered = true,
                IsSearch = true,
                IsSorted = true,
                Items = items
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="changedType"></param>
        /// <returns></returns>
        private async Task<bool> OnSaveAsync(Dict dict, ItemChangedType changedType)
        {
            if (dict == null) return false;

            string? PGName = changedType == ItemChangedType.Add ? Localizer["AddLogPGName"] : Localizer["EditLogPGName"]; //功能名稱
            string? PGCode = "";    //功能代碼, 待定
            string chgType = changedType == ItemChangedType.Add ? "I" : "U";
            string loginIP = (await WebClientService.GetClientInfo()).Ip!;

            var ret = await CommonService.SaveAsync(dict, (BootstrapAdmin.DataAccess.Models.ItemChangedTypeAction)changedType, AppContext.UserName, chgType, PGName, PGCode, loginIP);

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicts"></param>
        /// <returns></returns>
        private async Task<bool> OnDeleteAsync(IEnumerable<Dict> dicts)
        {
            string? PGName = Localizer["DelLogPGName"]; //功能名稱
            string? PGCode = "";    //功能代碼, 待定
            string chgType = "D";
            string loginIP = (await WebClientService.GetClientInfo()).Ip!;

            var ret = await CommonService.DeleteAsync(dicts, AppContext.UserName, chgType, PGName, PGCode, loginIP);

            return await Task.FromResult(ret);
        }

    }
}
