// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DictSearch
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public DictsSearchModel? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<DictsSearchModel> ValueChanged { get; set; }

        [Inject]
        [NotNull]
        private IStringLocalizer<DictSearch>? Localizer { get; set; }

        private IEnumerable<SelectedItem>? DefineSelectedItems { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            DefineSelectedItems = typeof(EnumDictDefine).ToSelectList(new SelectedItem("", Localizer["AllText"]));
        }
    }
}
