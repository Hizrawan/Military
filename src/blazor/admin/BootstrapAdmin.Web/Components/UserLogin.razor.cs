// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Library.Utilities;

using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class UserLogin
{
    private string? UserName { get; set; }

    private string? Password { get; set; }
    [Inject]
    [NotNull]
    private IStringLocalizer<UserLogin>? Localizer { get; set; }
}
