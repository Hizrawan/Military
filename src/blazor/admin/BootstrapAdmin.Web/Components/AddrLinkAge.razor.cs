// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class AddrLinkAge
{
    [Inject]
    [NotNull]
    private IStringLocalizer<AddrLinkAge>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private ISysAddr? SysAddrService { get; set; }

    private IEnumerable<SelectedItem>? ProItems { get; set; }

    private IEnumerable<SelectedItem>? CountyItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public string? Value { get; set; }


    //[Parameter]
    //public EventCallback<DictsSearchModel> ValueChanged { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        //TODO
        Value = "63000020001";

        ProItems = SysAddrService.GetAllPRO().ToSelectedItemList();
        CountyItems = SysAddrService.GetCountrys(Value.Substring(0, 5)).ToSelectedItemList();
    }
}
