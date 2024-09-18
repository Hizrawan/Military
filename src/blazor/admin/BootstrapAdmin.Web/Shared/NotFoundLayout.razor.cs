// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Pages.Admin;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Shared;

/// <summary>
/// NotFoundLayout 模板类
/// </summary>
public partial class NotFoundLayout
{
    private string Image { get; set; } = "images/404.png";

    private bool IsNotAuthorizated { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<NotFoundLayout>? Localizer { get; set; }
}
