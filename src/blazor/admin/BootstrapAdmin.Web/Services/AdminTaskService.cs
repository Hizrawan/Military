// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Jobs;
using Longbow.Tasks;

namespace BootstrapAdmin.Web.Services;

class AdminTaskService : BackgroundService
{
    private IDict DictService { get; set; }
    private ISendEmail SendEmailService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="sendEmail"></param>
    public AdminTaskService(IDict dict, ISendEmail sendEmail)
    {
        DictService = dict;
        SendEmailService = sendEmail;

    } 

    /// <summary>
    /// 運行任務
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() =>
    {
        TaskServicesManager.GetOrAdd("單次任務", token => Task.Delay(1000, token));
        //TODO, 先移除以下3個任務
        //TaskServicesManager.GetOrAdd("週期任務", token => Task.Delay(1000, token), TriggerBuilder.Default.WithInterval(10000).Build());
        //TaskServicesManager.GetOrAdd("Cron 任務", token => Task.Delay(1000, token), TriggerBuilder.Build(Cron.Secondly(5)));
        //TaskServicesManager.GetOrAdd("超時任務", token => Task.Delay(2000, token), TriggerBuilder.Default.WithTimeout(1000).WithInterval(1000).WithRepeatCount(2).Build());

        // 本機調試時此處會拋出異常，設定檔中默認開啟了任務持久化到物理檔，此處異常只有首次載入時會拋出
        // 此處異常是示例自訂任務內部未進行捕獲異常時任務仍然能繼續運行，不會導致整個進程崩潰退出
        // 此處代碼可注釋掉
        //TaskServicesManager.GetOrAdd("故障任務", token => throw new Exception("故障任務"));
        TaskServicesManager.GetOrAdd("取消任務", token => Task.Delay(1000, token)).Triggers.First().Enabled = false;

        // 創建任務並禁用
        TaskServicesManager.GetOrAdd("禁用任務", token => Task.Delay(1000, token)).Status = SchedulerStatus.Disabled;

        // 真實任務負責批次寫入資料執行腳本到日誌中
        TaskServicesManager.GetOrAdd<DBLogTask>("SQL日誌", TriggerBuilder.Build(Cron.Minutely()));

        // 真實任務負責週期性設置健康檢查結果開關為開啟  TODO 20240726 先移除檢查
        //TaskServicesManager.GetOrAdd("健康檢查", token => Task.FromResult(DictService.SaveHealthCheck()), TriggerBuilder.Build(Cron.Minutely(10)));

        //發送email任務
        //TaskServicesManager.GetOrAdd("發送Email", token => Task.FromResult(SendEmailService.SendEmail()), TriggerBuilder.Default.WithInterval(5000).Build());
    }, stoppingToken);
}
