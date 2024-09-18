// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;
using BootstrapAdmin.DataAccess.PetaPoco;
using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;
using BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages.News;
using BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages.PermissionControl;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPetaPocoDataAccessServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IDBManager, DBManagerService>();

        // 增加数据服务
        services.AddSingleton(typeof(IDataService<>), typeof(DefaultDataService<>));

        // 增加缓存服务
        services.AddCacheManager();

        // 增加业务服务
        services.AddSingleton<IAddress, AddressService>();
        services.AddSingleton<IApp, AppService>();
        services.AddSingleton<IDict, DictService>();
        services.AddSingleton<IException, ExceptionService>();
        services.AddSingleton<IGroup, GroupService>();
        services.AddSingleton<ILogin, LoginService>();
        services.AddSingleton<INavigation, NavigationService>();
        services.AddSingleton<IRole, RoleService>();
        services.AddSingleton<IUser, UserService>();
        services.AddSingleton<IBulletin, BulletinService>();
        services.AddSingleton<IDownloadArea, DownloadAreaService>();
        services.AddSingleton<IEducationTraining, EducationTrainingService>();
        services.AddSingleton<IEvent, EventService>();
        services.AddSingleton<ITrace, TraceService>();
        services.AddSingleton<ISysAddr, SysAddrService>();
        services.AddSingleton<IForm, FormService>();
        services.AddSingleton<IApLog, ApLogService>();
        services.AddSingleton(typeof(ICommon<>), typeof(CommonService<>));
        services.AddSingleton<IRepairFile, RepairFileService>();
        services.AddSingleton<IPhotoGallery, PhotoGalleryService>();
        services.AddSingleton<ILink, LinkService>();
        services.AddSingleton<ISendEmail, SendEmailService>();
        services.AddSingleton<ILeaveMessage, LeaveMessageService>();
        services.AddSingleton<IVolunteerNeed, VolunteerNeedService>();
        services.AddSingleton<IVolunteerRegistration, VolunteerRegistrationService>();
        services.AddSingleton<ISupervisor, SupervisorService>();
        services.AddSingleton<ISWU, SWUService>();
        services.AddSingleton<IUnitMaintenance, UnitMaintenanceService>();
        services.AddSingleton<IProgramManagement, ProgramManagementService>();


        return services;
    }
}
