// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core.Services;
using BootstrapAdmin.Web.HealthChecks;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Services.SMS;
using BootstrapAdmin.Web.Services.SMS.Tencent;
using BootstrapAdmin.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using BootstrapBlazor.DataAcces.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加示例幕後工作
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddBootstrapBlazorAdmin(this IServiceCollection services)
        {
            services.AddLogging(logging => logging.AddFileLogger().AddCloudLogger().AddDBLogger(ExceptionsHelper.Log));
            services.AddCors();
            services.AddResponseCompression();

            // 增加幕後工作
            services.AddTaskServices();
            services.AddHostedService<AdminTaskService>();

            // 增加 健康檢查服務
            services.AddAdminHealthChecks();

            // 增加 BootstrapBlazor 組件
            services.AddBootstrapBlazor();

            // 附加自己的 json 多語言文化資源檔 如 zh-TW.json
            services.ConfigureJsonLocalizationOptions(op =>
            {
                op.AdditionalJsonAssemblies = new Assembly[]
                {
                    typeof(BootstrapAdmin.Web.App).Assembly
                };
            });

            // 配置地理位置定位器
            services.ConfigureIPLocatorOption(op => op.LocatorFactory = LocatorHelper.CreateLocator);

            // 增加手機短信服務
            services.AddSingleton<ISMSProvider, TencentSMSProvider>();

            // 增加認證授權服務
            services.AddBootstrapAdminSecurity<AdminService>();

            // 增加 BootstrapApp 上下文服務
            services.AddScoped<BootstrapAppContext>();

            //
            services.AddScoped<AddLogByUserService>();

            // 增加 EFCore 資料服務
            //services.AddEFCoreDataAccessServices((provider, option) =>
            //{
            //    var configuration = provider.GetRequiredService<IConfiguration>();
            //    var connString = configuration.GetConnectionString("bb");
            //    option.UseSqlite(connString);
            //    option.EnableSensitiveDataLogging();
            //});
            //services.AddEFCoreDataAccessServices((provider, option) =>
            //{
            //    //需要引用 Microsoft.EntityFrameworkCore.Sqlite 包，操作 SQLite 資料庫
            //    var configuration = provider.GetRequiredService<IConfiguration>();
            //    var connString = configuration.GetConnectionString("bb");
            //    option.UseSqlite(connString);
            //});

            // 增加 FreeSql 資料服務
            //            services.AddFreeSql((provider, builder) =>
            //            {
            //                var configuration = provider.GetRequiredService<IConfiguration>();
            //                var connString = configuration.GetConnectionString("bb");
            //                builder.UseConnectionString(FreeSql.DataType.Sqlite, connString);
            //#if DEBUG
            //                調試sql語句輸出
            //                builder.UseMonitorCommand(cmd => System.Console.WriteLine(cmd.CommandText));
            //#endif
            //            });

            // 增加 PetaPoco 資料服務
            services.AddPetaPocoDataAccessServices();
            services.AddBootstrapBlazorTableExcelExport();
            return services;
        }
    }
}
