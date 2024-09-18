// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Longbow.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using PetaPoco;
using System.Data.Common;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class DictService : IDict
{
    private IDBManager DBManager { get; }

    private IStringLocalizer<DictService> Localizer { get; }

    private IApLog AplogService { get; }

    private string AppId { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="localizer"></param>
    /// <param name="aplogService"></param>
    /// <param name="configuration"></param>
    public DictService(IDBManager db, IStringLocalizer<DictService> localizer, IApLog aplogService, IConfiguration configuration)
    {
        DBManager = db;
        Localizer = localizer;
        AplogService = aplogService;
        AppId = configuration.GetValue("AppId", "BA")!;
    }

    public List<Dict> GetAll()
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT [dic].*
                FROM [Dicts] AS [dic]
                WHERE [dic].[IsDelete] = 0
            """;
        return db.Fetch<Dict>(sqlScript);
    }
    
    public List<Dict> GetHeaderAll()
    {
        CacheManager.Clear();
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT DISTINCT [dic].[Category]
                FROM [Dicts] AS [dic]
                WHERE [dic].[IsDelete] = 0
            """;
        return db.Fetch<Dict>(sqlScript);
    }

    public List<Dict> GetDetail(string category)
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT DISTINCT [dic].*
                FROM [Dicts] AS [dic]
                WHERE [dic].[IsDelete] = 0
                    AND [dic].[Category] = @Category
                ORDER BY [dic].[ShowOrder] ASC;
            """;
        return db.Fetch<Dict>(sqlScript, new { Category = category });
    }

    public async Task<Dictionary<string, (string Code, string?[] Params)>> GetKeyValueByCategoryAsync(string category)
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT [dic].*
                FROM [Dicts] AS [dic]
                WHERE [dic].[IsDelete] = 0
                    AND [dic].[Category] = @Category
            """;
        var dicts = await db.FetchAsync<Dict>(sqlScript, new { Category = category });
        return dicts.Select(dic => new KeyValuePair<string, (string Code, string?[] Params)>(
                dic.Name,
                (dic.Code, new string?[]
                {
                    dic.param1, dic.param2, dic.param3, dic.param4, dic.param5, dic.param6, dic.param7, dic.param8
                })
            )).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    //public async Task<bool> DeleteDicts(IEnumerable<Dict> dicts, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    //{
    //    bool ret;
    //    using var db = DBManager.Create();
    //    try
    //    {
    //        db.BeginTransaction();
    //        foreach (var dict in dicts)
    //        {
    //            string sqlScript = """
    //                UPDATE [Dicts]
    //                SET [IsDelete] = 1
    //                WHERE [ID] = @0
    //            """;
    //           var updRows = db.Execute(sqlScript, dict.Id);
    //            if (updRows >= 1)
    //            {
    //                await AplogService.SaveChangesAsync(dict, null, chgType, PGName, PGCode, userName, LoginIP);
    //            }
    //        }

    //        db.CompleteTransaction();
    //        ret = true;
    //    }
    //    catch (Exception)
    //    {
    //        db.AbortTransaction();
    //        throw;
    //    }
    //    if (ret)
    //    {
    //        CacheManager.Clear();
    //    }
    //    return ret;
    //}

    public Dictionary<string, string> GetApps()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == Localizer["App"]).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    public Dictionary<string, string> GetLogins()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == Localizer["DefaultUrl"]).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string GetCurrentLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["LoginUrl"] && d.Define == EnumDictDefine.System)?.Code ?? "Login";
    }

    public Dictionary<string, string> GetThemes()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == Localizer["WebTheme"]).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).ToDictionary(i => i.Key, i => i.Value);
    }

    /// <summary>
    /// 獲取 網站 Title 配置資訊
    /// </summary>
    /// <returns></returns>
    public string GetWebTitle()
    {
        var dicts = GetAll();
        string title = Localizer["WebTitle"];
        var name = dicts.FirstOrDefault(d => d.Category == Localizer["App"] && d.Code == AppId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == Localizer["WebTitle"]) ?? dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["WebTitle"]);
            title = dict?.Code ?? Localizer["WebTitle"];
        }
        return title;
    }

    /// <summary>
    /// 獲取網站 Footer 配置資訊
    /// </summary>
    /// <returns></returns>
    public string GetWebFooter()
    {
        var dicts = GetAll();
        string title = Localizer["WebFooter"];
        var name = dicts.FirstOrDefault(d => d.Category == Localizer["App"] && d.Code == AppId)?.Name;
        if (!string.IsNullOrEmpty(name))
        {
            var dict = dicts.FirstOrDefault(d => d.Category == name && d.Name == Localizer["WebFooter"]) ?? dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["WebFooter"]);
            title = dict?.Code ?? Localizer["WebTitle"];
        }
        return title;
    }

    /// <summary>
    /// 獲得 應用是否為演示模式
    /// </summary>
    /// <returns></returns>
    public bool IsDemo()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["DemoSystem"] && d.Define == EnumDictDefine.System)?.Code ?? "0";
        return code == "1";
    }

    /// <summary>
    /// 獲得當前授權碼是否有效可更改網站設置
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public bool AuthenticateDemo(string code)
    {
        var ret = false;
        if (!string.IsNullOrEmpty(code))
        {
            var dicts = GetAll();
            var salt = dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["AuthorizedSaltValue"] && d.Define == EnumDictDefine.System)?.Code;
            var authCode = dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["HashResult"] && d.Define == EnumDictDefine.System)?.Code;
            if (!string.IsNullOrEmpty(salt))
            {
                ret = LgbCryptography.ComputeHash(code, salt) == authCode;
            }
        }
        return ret;
    }

    /// <summary>
    /// 保存當前網站是否為演示系統
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    public bool SaveDemo(bool enable) => SaveDict(new Dict
    {
        Category = Localizer["WebSetting"],
        Name = Localizer["DemoSystem"],
        Code = enable ? "1" : "0",
        Define = EnumDictDefine.System
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    public bool SavDefaultApp(bool enable) => SaveDict(new Dict
    {
        Category = Localizer["WebSetting"],
        Name = Localizer["DefaultApp"],
        Code = enable ? "1" : "0",
        Define = EnumDictDefine.System
    });

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool SaveHealthCheck(bool enable = true) => SaveDict(new Dict
    {
        Category = Localizer["WebSetting"],
        Name = Localizer["HealthCheck"],
        Code = enable ? "1" : "0",
        Define = EnumDictDefine.System
    });

    /// <summary>
    /// 獲取當前網站 Cookie 保持時長
    /// </summary>
    /// <returns></returns>
    public int GetCookieExpiresPeriod()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["CookieTimeOut"] && d.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(code, out var ret);
        return ret;
    }

    private bool SaveDict(Dict dict, string? userName = null, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        using var db = DBManager.Create();

        //apLog 1, 更新前 model
        var oriDict = db.SingleOrDefault<Dict>("Where Category = @Category and Name = @Name", dict);
        IEnumerable<string> columns = new string[] { "Code" };  // 只檢索 Code 是否變化

        var ret = db.Update<Dict>("set Code = @Code where Category = @Category and Name = @Name", dict) == 1;
        if (ret)
        {
            //apLog 2, 保存 log
            AplogService.SaveChangesAsync(oriDict, dict, chgType, PGName, PGCode, userName, LoginIP, columns);
        }
        return ret;
    }

    public bool SaveLogin(string login) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["LoginUrl"], Code = login });

    public bool SaveTheme(string theme) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["UseTheme"], Code = theme });

    public bool SaveWebTitle(string title, string? userName = null, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["WebTitle"], Code = title }, userName, chgType, PGName, PGCode, LoginIP);

    public bool SaveWebFooter(string footer) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["WebFooter"], Code = footer });

    public bool SaveCookieExpiresPeriod(int expiresPeriod) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["CookieTimeOut"], Code = expiresPeriod.ToString() });

    public string? GetProfileUrl(string appId) => GetUrlByName(appId, Localizer["ProCenterUrl"]);

    public string? GetSettingsUrl(string appId) => GetUrlByName(appId, Localizer["SysSettingUrl"]);

    public string? GetNotificationUrl(string appId) => GetUrlByName(appId, Localizer["SysNoticeUrl"]);

    public bool GetEnableDefaultApp()
    {
        var dicts = GetAll();
        var code = dicts.FirstOrDefault(d => d.Category == Localizer["WebSetting"] && d.Name == Localizer["DefaultApp"])?.Code ?? "0";
        return code == "1";
    }

    private string? GetUrlByName(string appId, string dictName)
    {
        string? url = null;
        var dicts = GetAll();
        var appName = dicts.FirstOrDefault(d => d.Category == Localizer["App"] && d.Code == appId && d.Define == EnumDictDefine.System)?.Name;
        if (!string.IsNullOrEmpty(appName))
        {
            url = dicts.FirstOrDefault(d => d.Category == appName && d.Name == dictName && d.Define == EnumDictDefine.Customer)?.Code;
        }
        return url;
    }

    /// <summary>
    /// 獲取頭像路徑
    /// </summary>
    /// <returns></returns>
    public string GetIconFolderPath()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Name == Localizer["IconPath"] && d.Category == Localizer["IconUrl"] && d.Define == EnumDictDefine.System)?.Code ?? "/images/uploder/";
    }

    /// <summary>
    /// 獲取頭像路徑
    /// </summary>
    /// <returns></returns>
    public string GetDefaultIcon()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Name == Localizer["IconFile"] && d.Category == Localizer["IconUrl"] && d.Define == EnumDictDefine.System)?.Code ?? "default.jpg";
    }

    /// <summary>
    /// 通過指定 appId 獲得配置首頁位元址
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    public string? GetHomeUrlByAppId(string appId)
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(d => d.Category == Localizer["AppDefaultUrl"] && d.Name.Equals(appId, StringComparison.OrdinalIgnoreCase) && d.Define == EnumDictDefine.System)?.Code;
    }

    public bool GetAppSiderbar()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["SidebarStatus"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppSiderbar(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["SidebarStatus"], Code = value ? "1" : "0" });

    public bool GetAppTitle()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["CardTitleStatus"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppTitle(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["CardTitleStatus"], Code = value ? "1" : "0" });

    public bool GetAppFixHeader()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["FixHeader"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppFixHeader(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["FixHeader"], Code = value ? "1" : "0" });

    public bool GetAppHealthCheck()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["HealthCheck"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppHealthCheck(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["HealthCheck"], Code = value ? "1" : "0" });

    public bool GetAppMobileLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["SMSVerificationCodeLogin"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppMobileLogin(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["SMSVerificationCodeLogin"], Code = value ? "1" : "0" });

    public bool GetAppOAuthLogin()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["OAuthAuthenticationLogin"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAppOAuthLogin(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["OAuthAuthenticationLogin"], Code = value ? "1" : "0" });

    public bool GetAutoLockScreen()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["AutoLock"] && s.Define == EnumDictDefine.System)?.Code == "1";
    }

    public bool SaveAutoLockScreen(bool value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["AutoLock"], Code = value ? "1" : "0" });

    public int GetAutoLockScreenInterval()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["AutoLockTime"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveAutoLockScreenInterval(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["AutoLockTime"], Code = value.ToString() });

    public Dictionary<string, string> GetIpLocators()
    {
        var dicts = GetAll();
        return dicts.Where(d => d.Category == Localizer["LocationServices"]).Select(d => new KeyValuePair<string, string>(d.Code, d.Name)).OrderBy(i => i.Value).ToDictionary(i => i.Key, i => i.Value);
    }

    public string? GetIpLocator()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["IpLocationInterface"] && s.Define == EnumDictDefine.System)?.Code;
    }

    public bool SaveCurrentIp(string value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["IpLocationInterface"], Code = value });

    public int GetCookieExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["CookieTimeOut"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret;
    }

    public bool SaveCookieExpired(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["CookieTimeOut"], Code = value.ToString() });

    public int GetExceptionExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["ExceptionRetentionDuration"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveExceptionExpired(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["ExceptionRetentionDuration"], Code = value.ToString() });

    public int GetOperateExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["DurationOfOperationLogs"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveOperateExpired(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["DurationOfOperationLogs"], Code = value.ToString() });

    public int GetLoginExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["DurationOfLoginLogs"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveLoginExpired(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["DurationOfLoginLogs"], Code = value.ToString() });

    public int GetAccessExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["AccessLogRetentionDuration"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveAccessExpired(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["AccessLogRetentionDuration"], Code = value.ToString() });

    public int GetIPCacheExpired()
    {
        var dicts = GetAll();
        var value = dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["IPRequestCacheDuration"] && s.Define == EnumDictDefine.System)?.Code ?? "0";
        _ = int.TryParse(value, out var ret);
        return ret; ;
    }

    public bool SaveIPCacheExpired(int value) => SaveDict(new Dict { Category = Localizer["WebSetting"], Name = Localizer["IPRequestCacheDuration"], Code = value.ToString() });

    public Dictionary<string, string> GetClients()
    {
        var dicts = GetAll();
        return dicts.Where(s => s.Category == Localizer["App"] && s.Code != "BA").ToDictionary(s => s.Name, s => s.Code);
    }

    public string GetClientUrl(string name)
    {
        var dicts = GetAll();
        return dicts.Where(s => s.Category == Localizer["AppDefaultUrl"] && s.Name == name).FirstOrDefault()?.Code ?? "";
    }

    public bool ExistsAppId(string appId)
    {
        var dicts = GetAll();
        return dicts.Exists(s => s.Category == Localizer["App"] && s.Code == appId);
    }

    public bool SaveClient(ClientApp client)
    {
        var ret = false;
        if (!string.IsNullOrEmpty(client.AppId))
        {
            DeleteClient(client.AppId);
            using var db = DBManager.Create();
            try
            {
                db.BeginTransaction();
                var items = new List<Dict>()
                {
                    new Dict { Category = Localizer["App"], Name = client.AppName, Code = client.AppId, Define = EnumDictDefine.System },
                    new Dict { Category = Localizer["AppDefaultUrl"], Name = client.AppId, Code = client.HomeUrl, Define = EnumDictDefine.System },
                    new Dict { Category = client.AppId, Name = Localizer["WebFooter"], Code = client.Footer, Define = EnumDictDefine.Customer },
                    new Dict { Category = client.AppId, Name = Localizer["WebTitle"], Code = client.Title, Define = EnumDictDefine.Customer },
                    new Dict { Category = client.AppId, Name = Localizer["favicon"], Code = client.Favicon, Define = EnumDictDefine.Customer },
                    new Dict { Category = client.AppId, Name = Localizer["WebIcon"], Code = client.Icon, Define = EnumDictDefine.Customer },
                    new Dict { Category = client.AppId, Name = Localizer["ProCenterUrl"], Code = client.ProfileUrl, Define = EnumDictDefine.Customer },
                    new Dict { Category = client.AppId, Name = Localizer["SysSettingUrl"], Code = client.SettingsUrl, Define = EnumDictDefine.Customer },
                    new Dict { Category = client.AppId, Name = Localizer["SysNoticeUrl"], Code = client.NotificationUrl, Define = EnumDictDefine.Customer }
                };
                db.InsertBatch(items.Where(i => !string.IsNullOrEmpty(i.Code)));
                db.CompleteTransaction();
                ret = true;
            }
            catch (DbException)
            {
                db.AbortTransaction();
                throw;
            }
        }
        return ret;
    }

    public ClientApp GetClient(string appId)
    {
        var dicts = GetAll();
        return new ClientApp()
        {
            AppId = appId,
            AppName = dicts.FirstOrDefault(s => s.Category == Localizer["App"] && s.Code == appId)?.Name,
            HomeUrl = dicts.FirstOrDefault(s => s.Category == Localizer["AppDefaultUrl"] && s.Name == appId)?.Code,
            ProfileUrl = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["ProCenterUrl"])?.Code,
            SettingsUrl = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["SysSettingUrl"])?.Code,
            NotificationUrl = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["SysNoticeUrl"])?.Code,
            Title = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["WebTitle"])?.Code,
            Footer = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["WebFooter"])?.Code,
            Icon = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["WebIcon"])?.Code,
            Favicon = dicts.FirstOrDefault(s => s.Category == appId && s.Name == Localizer["favicon"])?.Code,
        };
    }

    public bool DeleteClient(string appId)
    {
        bool ret;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from Dicts where Category=@0 and Name=@1 and Define=@2", Localizer["AppDefaultUrl"], appId, EnumDictDefine.System);
            db.Execute("delete from Dicts where Category=@0 and Code=@1 and Define=@2", Localizer["App"], appId, EnumDictDefine.System);
            db.Execute("delete from Dicts where Category=@Category and Name in (@Names)", new
            {
                Category = appId,
                Names = new List<string>
                {
                    Localizer["WebTitle"],
                    Localizer["WebFooter"],
                    Localizer["favicon"],
                    Localizer["WebIcon"],
                    Localizer["ProCenterUrl"],
                    Localizer["SysSettingUrl"],
                    Localizer["SysNoticeUrl"]
                }
            });
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string? GetIpLocatorName()
    {
        var dicts = GetAll();
        return dicts.FirstOrDefault(s => s.Category == Localizer["WebSetting"] && s.Name == Localizer["IpLocationInterface"] && s.Define == EnumDictDefine.System)?.Code;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string? GetIpLocatorUrl(string? name)
    {
        var dicts = GetAll();
        return string.IsNullOrWhiteSpace(name) ? null : dicts.FirstOrDefault(s => s.Category == Localizer["GeographicLocation"] && s.Name == name && s.Define == EnumDictDefine.System)?.Code;
    }

    //TODO, 改用公共服務的保存方法, 此處暫存
    //public async Task<bool> SaveAsync(Dict dict, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    //{
    //    if (dict == null) return false;

    //    using var db = DBManager.Create();
    //    if (changedType == ItemChangedTypeAction.Add)
    //    {
    //        dict.CreateBy = userName;
    //        dict.CreateDate = DateTime.Now;
    //        var obj = await db.InsertAsync(dict);

    //        //apLog 2, 保存 log
    //        if (obj != null)
    //        {
    //            await AplogService.SaveChangesAsync(null, dict, chgType, PGName, PGCode, userName, LoginIP);
    //        }
    //    }
    //    else
    //    {
    //        //apLog 1, 更新前 model
    //        var oriDict = db.SingleOrDefault<Dict>("Where Id = @0", dict.Id);

    //        dict.ModifyBy = userName;
    //        dict.ModifyDate = DateTime.Now;
    //        var updRows = await db.UpdateAsync(dict);

    //        //apLog 2, 保存 log
    //        if (updRows >= 1)
    //        {
    //            await AplogService.SaveChangesAsync(oriDict, dict, chgType, PGName, PGCode, userName, LoginIP);
    //        }
    //    }

    //    return true;
    //}
}
