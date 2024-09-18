// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Utils;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ErrorSearch
    {
        [Inject]
        [NotNull]
        private IStringLocalizer<ErrorSearch>? Localizer { get; set; }

        private List<SelectedItem>? Items { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public ErrorSearchModel? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<ErrorSearchModel> ValueChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Items = new List<SelectedItem>
            {
                new SelectedItem("", Localizer["AllText"])
            };
            Items.AddRange(LookupHelper.GetExceptionCategory());
        }
    }
}
