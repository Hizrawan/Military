﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
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
    public partial class UserSearch
    {
        [Inject]
        [NotNull]
        private IStringLocalizer<UserSearch>? Localizer { get; set; }

        private IEnumerable<SelectedItem>? Items { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        [NotNull]
        public UsersSearchModel? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public EventCallback<UsersSearchModel> ValueChanged { get; set; }
    }
}
