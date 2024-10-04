// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetaPoco;
using PetaPoco.Providers;
using System.Collections.Specialized;
using System.Data.Common;
using System.Text;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class DBManagerService : IDBManager
{
    private IConfiguration Configuration { get; }

    private ILogger<DBManagerService> Logger { get; }

    private IWebHostEnvironment WebHost { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <param name="webHost"></param>
    public DBManagerService(IConfiguration configuration, ILogger<DBManagerService> logger, IWebHostEnvironment webHost)
        => (Configuration, Logger, WebHost) = (configuration, logger, webHost);

    /// <summary>
    /// 創建 IDatabase 實例方法
    /// </summary>
    /// <param name="connectionName">連接字串鍵值</param>
    /// <param name="keepAlive"></param>
    /// <returns></returns>
    public IDatabase Create(string connectionName = "MILITARYDB", bool keepAlive = false)
    {
        var conn = Configuration.GetConnectionString(connectionName) ?? throw new ArgumentNullException(nameof(connectionName));

        var option = DatabaseConfiguration.Build();
        option.UsingDefaultMapper<BootstrapAdminConventionMapper>();

        // connectionstring
        option.UsingConnectionString(conn);

        // provider
        //option.UsingProvider<SQLiteDatabaseProvider>();   // SQLite
        option.UsingProvider<SqlServerDatabaseProvider>();


        var db = new Database(option) { KeepConnectionAlive = keepAlive };

        db.ExceptionThrown += (sender, e) =>
        {
            var message = e.Exception.Format(new NameValueCollection()
            {
                [nameof(db.LastCommand)] = db.LastCommand,
                [nameof(db.LastArgs)] = string.Join(",", db.LastArgs)
            });
            Logger.LogError(e.Exception, "{Message}", message);
        };
        if (WebHost.IsDevelopment())
        {
            db.CommandExecuted += (sender, args) =>
            {
                var parameters = new StringBuilder();
                foreach (DbParameter p in args.Command.Parameters)
                {
                    parameters.AppendFormat("{0}: {1}  ", p.ParameterName, p.Value);
                }
                Logger.LogInformation("{CommandText}", args.Command.CommandText);
                Logger.LogInformation("{CommandArgs}", parameters.ToString());
            };
        };
        return db;
    }
}
