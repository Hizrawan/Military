// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Models;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class TaskEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public TasksModel? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public EventCallback<TasksModel> ValueChanged { get; set; }

    private string? TaskName;

    [NotNull]
    private List<SelectedItem>? Items { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<TaskEditor>? Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        TaskName = Localizer["TaskName"];

        Items = new List<SelectedItem>
        {
            new(Longbow.Tasks.Cron.Secondly(5), Localizer["TaskRunSecondly", 5]),
            new(Longbow.Tasks.Cron.Minutely(1), Localizer["TaskRunMinutely", 1]),
            new(Longbow.Tasks.Cron.Minutely(5), Localizer["TaskRunMinutely", 5]),
        };

        if (string.IsNullOrEmpty(Value.Trigger))
        {
            Value.Trigger = Items.First().Value;
        }
    }
}
