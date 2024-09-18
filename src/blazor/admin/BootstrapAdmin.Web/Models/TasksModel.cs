// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Longbow.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class TasksModel
{
    /// <summary>
    /// 任務名稱
    /// </summary>
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 上次執行時間
    /// </summary>
    public DateTimeOffset? LastRuntime { get; set; }

    /// <summary>
    /// 下次執行時間
    /// </summary>
    public DateTimeOffset? NextRuntime { get; set; }

    /// <summary>
    /// 觸發條件
    /// </summary>
    [NotNull]
    public string? Trigger { get; set; }

    /// <summary>
    /// 執行結果
    /// </summary>
    [NotNull]
    public TriggerResult LastRunResult { get; set; }

    /// <summary>
    /// 任務狀態
    /// </summary>
    [NotNull]
    public SchedulerStatus Status { get; set; }
}
