// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BootstrapAdmin.Web.HealthChecks;

/// <summary>
/// 資料庫檢查類
/// </summary>
class DBHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly Func<IConfiguration, string, string?> ConnectionStringResolve = (c, name) => string.IsNullOrEmpty(name)
        ? c.GetSection("ConnectionStrings").GetChildren().FirstOrDefault()?.Value
        : c.GetConnectionString(name);

    /// <summary>
    /// 構造函數
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="httpContextAccessor"></param>
    public DBHealthCheck(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 非同步檢查方法
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        //var db = _configuration.GetSection("DB").GetChildren()
        //    .Select(config => new DbOption()
        //    {
        //        Enabled = bool.TryParse(config["Enabled"], out var en) ? en : false,
        //        ProviderName = config["ProviderName"],
        //        Widget = config["Widget"],
        //        ConnectionString = ConnectionStringResolve(config.GetSection("ConnectionStrings").Exists() ? config : _configuration, string.Empty)
        //    }).FirstOrDefault(i => i.Enabled) ?? new DbOption()
        //    {
        //        Enabled = true,
        //        ProviderName = Longbow.Data.DatabaseProviderType.SqlServer.ToString(),
        //        Widget = typeof(User).Assembly.FullName,
        //        ConnectionString = Longbow.Data.DbManager.GetConnectionString()
        //    };

        //// 檢查 當前使用者 帳戶許可權
        //var loginUser = _httpContextAccessor.HttpContext?.User.Identity?.Name;
        //var userName = loginUser ?? "Admin";
        //var dictsCount = 0;
        //var menusCount = 0;
        //var roles = string.Empty;
        //var displayName = string.Empty;
        //var healths = false;
        //Exception? error = null;
        //try
        //{
        //    var user = UserHelper.RetrieveUserByUserName(userName);
        //    displayName = user?.DisplayName ?? string.Empty;
        //    roles = string.Join(",", RoleHelper.RetrievesByUserName(userName) ?? new string[0]);
        //    menusCount = MenuHelper.RetrieveMenusByUserName(userName)?.Count() ?? 0;
        //    dictsCount = DictHelper.RetrieveDicts()?.Count() ?? 0;
        //    healths = user != null && !string.IsNullOrEmpty(roles) && menusCount > 0 && dictsCount > 0;

        //    // 檢查資料庫是否可寫
        //    var dict = new BootstrapDict()
        //    {
        //        Category = "DB-Check",
        //        Name = "WriteTest",
        //        Code = "1"
        //    };
        //    if (DictHelper.Save(dict) && !string.IsNullOrEmpty(dict.Id)) DictHelper.Delete(new string[] { dict.Id });
        //}
        //catch (Exception ex)
        //{
        //    error = ex;
        //}
        //var data = new Dictionary<string, object>()
        //{
        //    { "ConnectionString", db.ConnectionString ?? string.Empty },
        //    { "Reference", DbContextManager.Create<Dict>()?.GetType().Assembly.FullName ?? db.Widget ?? string.Empty },
        //    { "DbType", db?.ProviderName ?? string.Empty },
        //    { "Dicts", dictsCount },
        //    { "LoginName", userName },
        //    { "DisplayName", displayName },
        //    { "Roles", roles },
        //    { "Navigations", menusCount }
        //};

        //if (string.IsNullOrEmpty(db?.ConnectionString))
        //{
        //    // 未啟用連接字串
        //    data["ConnectionString"] = "未配置資料庫連接字串";
        //    return Task.FromResult(HealthCheckResult.Unhealthy("Error", null, data));
        //}

        //if (DbContextManager.Exception != null) error = DbContextManager.Exception;
        //if (error != null)
        //{
        //    data.Add("Exception", error.Message);

        //    if (error.Message.Contains("SQLite Error 8: 'attempt to write a readonly database'.")) data.Add("解決辦法", "更改資料庫檔為可讀，並授予進程可寫許可權");
        //    if (error.Message.Contains("Could not load", StringComparison.OrdinalIgnoreCase)) data.Add("解決辦法", "Nuget 引用相對應的資料庫驅動 dll");

        //    // UNDONE: Json 序列化迴圈引用導致異常 NET 5.0 修復此問題
        //    // 目前使用 new Exception() 臨時解決
        //    return Task.FromResult(HealthCheckResult.Unhealthy("Error", new Exception(error.Message), data));
        //}

        //return healths ? Task.FromResult(HealthCheckResult.Healthy("Ok", data)) : Task.FromResult(HealthCheckResult.Degraded("Failed", null, data));

        return Task.FromResult(HealthCheckResult.Healthy("Ok"));
    }

    private class DbOption
    {
        public bool Enabled { get; set; }
        public string? ProviderName { get; set; }
        public string? Widget { get; set; }
        public string? ConnectionString { get; set; }
    }
}
