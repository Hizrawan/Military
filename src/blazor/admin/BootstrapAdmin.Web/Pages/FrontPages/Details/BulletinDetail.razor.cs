// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;

namespace BootstrapAdmin.Web.Pages.FrontPages.Details;
/// <summary>
/// 
/// </summary>
public partial class BulletinDetail
{
    private Bulletin? result { get; set; }

    [CascadingParameter(Name = "BodyContext")]

    private object? Bulletins { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override Task OnInitializedAsync()
    {
        if (Bulletins is Bulletin test)
        {
            result = test;

        }
        return Task.CompletedTask;
    }
}
