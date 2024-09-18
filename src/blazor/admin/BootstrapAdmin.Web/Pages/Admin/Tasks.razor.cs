// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using Longbow.Tasks;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Tasks
{
    private List<TasksModel> SelectedRows { get; set; } = new List<TasksModel>();

    private static IEnumerable<string> Jobs => new string[]
    {
        "單次任務",
        "週期任務",
        "Cron 任務",
        "超時任務",
        "取消任務",
        "禁用任務",
        "SQL日誌",
        "健康檢查"
    };

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Tasks>? Localizer { get; set; }

    private bool IsDemo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        IsDemo = DictService.IsDemo();
    }

    private Task<QueryData<TasksModel>> OnQueryAsync(QueryPageOptions options)
    {
        var tasks = TaskServicesManager.ToList().ToTasksModelList();
        if (options.SortList != null && options.SortList.Any())
        {
            tasks = tasks.Sort(options.SortList).ToList();
        }
        var model = tasks.FirstOrDefault(i => i.Name == SelectedRows.FirstOrDefault()?.Name);
        SelectedRows.Clear();
        if (model != null)
        {
            SelectedRows.Add(model);
        }
        return Task.FromResult(new QueryData<TasksModel>()
        {
            Items = tasks
        });
    }

    private static Task<bool> OnSaveAsync(TasksModel model, ItemChangedType changedType)
    {
        var taskExecutor = new DefaultTaskExecutor();
        TaskServicesManager.Remove(model.Name);
        TaskServicesManager.GetOrAdd(model.Name, token => taskExecutor.Execute(token), TriggerBuilder.Build(model.Trigger));
        return Task.FromResult(true);
    }

    private Task<bool> OnDeleteAsync(IEnumerable<TasksModel> models)
    {
        // 演示模式下禁止刪除內置任務
        if (IsDemo)
        {
            var m = models.ToList();
            m.RemoveAll(m => Jobs.Any(i => i == m.Name));
            models = m;
        }

        // 迴圈刪除任務
        foreach (var model in models)
        {
            TaskServicesManager.Remove(model.Name);
        }
        return Task.FromResult(true);
    }

    private bool OnShowButtonCallback(TasksModel model) => !IsDemo && !Jobs.Any(i => i == model.Name);

    private static Color GetResultColor(TriggerResult result) => result switch
    {
        TriggerResult.Success => Color.Success,
        TriggerResult.Error => Color.Danger,
        TriggerResult.Timeout => Color.Warning,
        TriggerResult.Cancelled => Color.Dark,
        _ => Color.Primary
    };

    private static string FormatResult(TriggerResult result) => result switch
    {
        TriggerResult.Success => "成功",
        TriggerResult.Error => "故障",
        TriggerResult.Timeout => "超時",
        TriggerResult.Cancelled => "取消",
        _ => "未知狀態"
    };

    private static Color GetStatusColor(SchedulerStatus status) => status switch
    {
        SchedulerStatus.Running => Color.Success,
        SchedulerStatus.Ready => Color.Danger,
        SchedulerStatus.Disabled => Color.Danger,
        _ => Color.Primary
    };

    private static string FormatStatus(SchedulerStatus status) => status switch
    {
        SchedulerStatus.Running => "運行中",
        SchedulerStatus.Ready => "已停止",
        SchedulerStatus.Disabled => "禁用",
        _ => "未知狀態"
    };

    private static string GetStatusIcon(SchedulerStatus status) => status switch
    {
        SchedulerStatus.Running => "fa-solid fa-play-circle",
        SchedulerStatus.Ready => "fa-solid fa-stop-circle",
        SchedulerStatus.Disabled => "fa-solid fa-times-circle",
        _ => "未知狀態"
    };

    private Task OnPause(TasksModel model)
    {
        var task = TaskServicesManager.ToList().FirstOrDefault(i => i.Name == model.Name);
        if (task != null)
        {
            task.Status = SchedulerStatus.Ready;
        }
        SelectedRows.Clear();
        SelectedRows.Add(model);
        return Task.CompletedTask;
    }

    private Task OnRun(TasksModel model)
    {
        var task = TaskServicesManager.ToList().FirstOrDefault(i => i.Name == model.Name);
        if (task != null)
        {
            task.Status = SchedulerStatus.Running;
        }
        SelectedRows.Clear();
        SelectedRows.Add(model);
        return Task.CompletedTask;
    }

    private async Task OnLog(TasksModel model)
    {
        var option = new DialogOption()
        {
            Class = "modal-dialog-task",
            Title = $"{model.Name} - 日誌視窗(最新 20 條)",
            Component = BootstrapDynamicComponent.CreateComponent<TaskInfo>(new Dictionary<string, object?>
            {
                [nameof(TaskInfo.Model)] = model
            })
        };
        await DialogService.Show(option);
    }

    private static bool OnCheckTaskStatus(TasksModel model) => model.Status != SchedulerStatus.Disabled;

    class DefaultTaskExecutor : ITask
    {
        /// <summary>
        /// 任務執行方法
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Execute(CancellationToken cancellationToken) => Task.Delay(1000, cancellationToken);
    }
}
