// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// Dict 字典表介面
/// </summary>
public interface IDict
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<Dict> GetAll();
    /// <summary>
    /// 
    /// </summary>
    List<Dict> GetHeaderAll();
    /// <summary>
    /// 
    /// </summary>
    List<Dict> GetDetail(string category);
    /// <summary>
    /// 獲得 指定目錄的所有 Key/Value, 參數 集合
    /// </summary>
    /// <returns></returns>
    Task<Dictionary<string, (string Code, string?[] Params)>> GetKeyValueByCategoryAsync(string category);

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="dicts"></param>
    ///// <param name="userName"></param>
    ///// <param name="chgType"></param>
    ///// <param name="PGName"></param>
    ///// <param name="PGCode"></param>
    ///// <param name="LoginIP"></param>
    ///// <returns></returns>
    //Task<bool> DeleteDicts(IEnumerable<Dict> dicts, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string? GetIpLocatorName();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    string? GetIpLocatorUrl(string? name);

    /// <summary>
    /// 獲得 配置所有的 App 集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetApps();

    /// <summary>
    /// 獲得 配置所有的登錄頁集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetLogins();

    /// <summary>
    /// 獲得 當前配置登錄頁
    /// </summary>
    /// <returns></returns>
    string GetCurrentLogin();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    bool SaveLogin(string login);

    /// <summary>
    /// 獲得 配置所有的主題集合
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetThemes();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="theme"></param>
    /// <returns></returns>
    bool SaveTheme(string theme);

    /// <summary>
    /// 獲取當前系統組態是否為演示模式
    /// </summary>
    /// <returns></returns>
    bool IsDemo();

    /// <summary>
    /// 保存當前網站是否為演示系統
    /// </summary>
    /// <param name="isDemo"></param>
    /// <returns></returns>
    bool SaveDemo(bool isDemo);

    /// <summary>
    /// 保存是否開啟默認應用設置
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    bool SavDefaultApp(bool enabled);

    /// <summary>
    /// 保存健康檢查
    /// </summary>
    /// <returns></returns>
    bool SaveHealthCheck(bool enable = true);

    /// <summary>
    /// 獲得當前授權碼是否有效可更改網站設置
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    bool AuthenticateDemo(string code);

    /// <summary>
    /// 獲取 網站 Title 配置資訊
    /// </summary>
    /// <returns></returns>
    string GetWebTitle();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    bool SaveWebTitle(string title, string? userName = null, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 獲取網站 Footer 配置資訊
    /// </summary>
    /// <returns></returns>
    string GetWebFooter();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="footer"></param>
    /// <returns></returns>
    bool SaveWebFooter(string footer);

    /// <summary>
    /// 獲得 Cookie 登錄持久化時長
    /// </summary>
    /// <returns></returns>
    int GetCookieExpiresPeriod();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expiresPeriod"></param>
    /// <returns></returns>
    bool SaveCookieExpiresPeriod(int expiresPeriod);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetProfileUrl(string appId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetSettingsUrl(string appId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetNotificationUrl(string appId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string GetIconFolderPath();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    string GetDefaultIcon();

    /// <summary>
    /// 通過指定 appId 獲得配置首頁位址
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    string? GetHomeUrlByAppId(string appId);

    /// <summary>
    /// 是否開啟默認應用
    /// </summary>
    /// <returns></returns>
    bool GetEnableDefaultApp();

    /// <summary>
    /// 是否開啟側邊欄設置
    /// </summary>
    /// <returns></returns>
    bool GetAppSiderbar();

    /// <summary>
    /// 保存側邊欄設置
    /// </summary>
    /// <returns></returns>
    bool SaveAppSiderbar(bool value);

    /// <summary>
    /// 是否開啟標題設置
    /// </summary>
    /// <returns></returns>
    bool GetAppTitle();

    /// <summary>
    /// 保存標題設置
    /// </summary>
    /// <returns></returns>
    bool SaveAppTitle(bool value);

    /// <summary>
    /// 是否開啟固定表頭設置
    /// </summary>
    /// <returns></returns>
    bool GetAppFixHeader();

    /// <summary>
    /// 保存固定表頭設置
    /// </summary>
    /// <returns></returns>
    bool SaveAppFixHeader(bool value);

    /// <summary>
    /// 是否開啟健康檢查設置
    /// </summary>
    /// <returns></returns>
    bool GetAppHealthCheck();

    /// <summary>
    /// 保存健康檢查設置
    /// </summary>
    /// <returns></returns>
    bool SaveAppHealthCheck(bool value);

    /// <summary>
    /// 是否開啟手機認證設置
    /// </summary>
    /// <returns></returns>
    bool GetAppMobileLogin();

    /// <summary>
    /// 保存手機認證設置
    /// </summary>
    /// <returns></returns>
    bool SaveAppMobileLogin(bool value);

    /// <summary>
    /// 是否開啟 OAuth 認證設置
    /// </summary>
    /// <returns></returns>
    bool GetAppOAuthLogin();

    /// <summary>
    /// 保存 OAuth 認證設置
    /// </summary>
    /// <returns></returns>
    bool SaveAppOAuthLogin(bool value);

    /// <summary>
    /// 是否開啟自動鎖屏設置
    /// </summary>
    /// <returns></returns>
    bool GetAutoLockScreen();

    /// <summary>
    /// 保存自動鎖屏設置
    /// </summary>
    /// <returns></returns>
    bool SaveAutoLockScreen(bool value);

    /// <summary>
    /// 獲得自動鎖屏間隔時間
    /// </summary>
    /// <returns></returns>
    int GetAutoLockScreenInterval();

    /// <summary>
    /// 保存自動鎖屏間隔時間
    /// </summary>
    /// <returns></returns>
    bool SaveAutoLockScreenInterval(int value);

    /// <summary>
    /// 獲得地理位置服務
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetIpLocators();

    /// <summary>
    /// 獲得當前地理位置服務
    /// </summary>
    /// <returns></returns>
    string? GetIpLocator();

    /// <summary>
    /// 設置當前地理位置服務
    /// </summary>
    /// <returns></returns>
    bool SaveCurrentIp(string value);

    /// <summary>
    /// 獲得 Cookie 過期時間
    /// </summary>
    /// <returns></returns>
    int GetCookieExpired();

    /// <summary>
    /// 設置 Cookie 過期時間
    /// </summary>
    /// <returns></returns>
    bool SaveCookieExpired(int value);

    /// <summary>
    /// 獲得程式異常保留時長
    /// </summary>
    /// <returns></returns>
    int GetExceptionExpired();

    /// <summary>
    /// 設置程式異常保留時長
    /// </summary>
    /// <returns></returns>
    bool SaveExceptionExpired(int value);

    /// <summary>
    /// 獲得操作日誌保留時長
    /// </summary>
    /// <returns></returns>
    int GetOperateExpired();

    /// <summary>
    /// 設置操作日誌保留時長
    /// </summary>
    /// <returns></returns>
    bool SaveOperateExpired(int value);

    /// <summary>
    /// 獲得登錄日誌保留時長
    /// </summary>
    /// <returns></returns>
    int GetLoginExpired();

    /// <summary>
    /// 設置登錄日誌保留時長
    /// </summary>
    /// <returns></returns>
    bool SaveLoginExpired(int value);

    /// <summary>
    /// 獲得訪問日誌保留時長
    /// </summary>
    /// <returns></returns>
    int GetAccessExpired();

    /// <summary>
    /// 設置訪問日誌保留時長
    /// </summary>
    /// <returns></returns>
    bool SaveAccessExpired(int value);

    /// <summary>
    /// 獲得 IP 請求緩存時長
    /// </summary>
    /// <returns></returns>
    int GetIPCacheExpired();

    /// <summary>
    /// 設置 IP 請求緩存時長
    /// </summary>
    /// <returns></returns>
    bool SaveIPCacheExpired(int value);

    /// <summary>
    /// 獲得前臺應用
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string>? GetClients();

    /// <summary>
    /// 獲得前臺應用
    /// </summary>
    /// <returns></returns>
    string GetClientUrl(string name);

    /// <summary>
    /// 獲得前臺應用
    /// </summary>
    /// <returns></returns>
    bool ExistsAppId(string appId);

    /// <summary>
    /// 添加前臺應用
    /// </summary>
    /// <returns></returns>
    bool SaveClient(ClientApp client);

    /// <summary>
    /// 獲得前臺應用配置
    /// </summary>
    /// <returns></returns>
    ClientApp GetClient(string appId);

    /// <summary>
    /// 刪除前臺應用配置
    /// </summary>
    /// <returns></returns>
    bool DeleteClient(string appId);
}
