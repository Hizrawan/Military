// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class EmailLogSearch
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public EmailLogSearchModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<EmailLogSearchModel> ValueChanged { get; set; }
     
    [Inject]
    [NotNull]
    private IStringLocalizer<EmailLogSearch>? Localizer { get; set; }
     
    [Inject]
    [NotNull]
    private ISendEmail? SendEmailService { get; set; }
 
    [NotNull]
    [Inject]
    private IDict? DictService { get; set; }
  

    private List<SelectedItem>? EmailTypeSelect { get; set; }

    private List<SelectedItem>? EmailStatusSelect { get; set; }
    
    /// <summary>
    /// [覆寫方法] [非同步] 元件初始化
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {

        EmailTypeSelect = (await DictService.GetKeyValueByCategoryAsync(nameof(SendEmail.EmailType)))
            .Select(kvp => new SelectedItem(kvp.Value.Code, kvp.Key))
            .ToList();
        EmailTypeSelect.Insert(0, new SelectedItem("", Localizer["AllText"]));


        EmailStatusSelect = (await DictService.GetKeyValueByCategoryAsync(nameof(SendEmail.EmailStatus)))
            .Select(kvp => new SelectedItem(kvp.Value.Code, kvp.Key))
            .ToList();
        EmailStatusSelect.Insert(0, new SelectedItem("", Localizer["AllText"]));

        await base.OnInitializedAsync();
    }
}
