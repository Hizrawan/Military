// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Pages.Admin;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Services;

/// <summary>
/// 因在更新資料庫時, 需要引用當前AppContext.UserName 等訊息
/// </summary>
public class AddLogByUserService
{
    private ICommon<RepairLog> RepairLogService { get; set; }

    private BootstrapAppContext? AppContext { get; set; }

    private IStringLocalizer<AddLogByUserService>? Localizer { get; set; }

    private WebClientService? WebClientService { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repairLogService"></param>
    /// <param name="appContext"></param>
    /// <param name="localizer"></param>
    /// <param name="webClientService"></param>
    public AddLogByUserService(ICommon<RepairLog> repairLogService, BootstrapAppContext appContext, IStringLocalizer<AddLogByUserService> localizer, WebClientService webClientService)
    {
        RepairLogService = repairLogService;
        AppContext = appContext;
        Localizer = localizer;
        WebClientService = webClientService;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repairId"></param>
    /// <param name="oldRepairStatus"></param>
    /// <param name="newRepairStatus"></param>
    /// <returns></returns>
    public async Task AddRepairLog(string? repairId, string? oldRepairStatus, string? newRepairStatus)
    {
        if (string.IsNullOrEmpty(repairId) || string.IsNullOrEmpty(oldRepairStatus) || string.IsNullOrEmpty(newRepairStatus))
        {
            return;
        }
        var repairLog = new RepairLog
        {
            RepairId = int.Parse(repairId),
            OldRepairStatus = oldRepairStatus,
            NewRepairStatus = newRepairStatus,
            CreateBy = AppContext!.UserName,
            CreateDate = DateTime.Now,
        };

        string? PGName = Localizer!["AddRepairLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = "I";   //這邊報修單會采用新增空單的方式來修改, 因此, Id不為空視為修改
        string loginIP = (await WebClientService!.GetClientInfo()).Ip!;

        //改成調用公共方法
        await RepairLogService.SaveAsync(repairLog, (BootstrapAdmin.DataAccess.Models.ItemChangedTypeAction)ItemChangedType.Add, AppContext.UserName, chgType, PGName, PGCode, loginIP);

    }

}
