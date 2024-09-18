// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IUser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    User? GetUserByUserName(string? userName);

    /// <summary>
    /// 通過使用者名獲取角色列表
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    List<string> GetRoles(string userName);

    /// <summary>
    /// 通過使用者名獲得授權 App 集合
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    List<string> GetApps(string userName);

    /// <summary>
    /// 通過使用者名獲得指定的前臺 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    string? GetAppIdByUserName(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    List<string> GetUsersByGroupId(string? groupId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    bool SaveUsersByGroupId(string? groupId, IEnumerable<string> userIds);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    List<string> GetUsersByRoleId(string? roleId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userIds"></param>
    /// <returns></returns>
    bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds);

    /// <summary>
    /// 更新密碼方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    bool ChangePassword(string userName, string password, string newPassword);

    /// <summary>
    /// 保存顯示名稱方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <returns></returns>
    bool SaveDisplayName(string userName, string displayName);

    /// <summary>
    /// 保存使用者主題方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="theme"></param>
    /// <returns></returns>
    bool SaveTheme(string userName, string theme);

    /// <summary>
    /// 保存使用者頭像方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="logo"></param>
    /// <returns></returns>
    bool SaveLogo(string userName, string? logo);

    /// <summary>
    /// 獲得所有使用者
    /// </summary>
    /// <returns></returns>
    List<User> GetAll();

    /// <summary>
    /// 保存指定使用者的默認 App
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="app"></param>
    bool SaveApp(string userName, string app);

    /// <summary>
    /// 認證方法
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="authType">1, 登入驗證時(authType = Login), 要加上IsEnable 以及 IsDelete的判斷; 修改密碼時(authType = null), 暫不判斷 IsEnable 及IsDelete</param>
    /// <returns></returns>
    bool Authenticate(string userName, string password, string? authType = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <param name="password"></param>
    /// <param name="groupsId"></param>
    /// <param name="userSID"></param>
    /// <param name="userEmail"></param>
    /// <param name="userTelPhone"></param>
    /// <param name="userCellPhone"></param>
    /// <param name="isEnabled"></param>
    /// <param name="userType"></param>
    /// <param name="vendorId"></param>
    /// <param name="chgUserName">操作人員</param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>    
    bool SaveUser(string userName, string displayName, string password, int groupsId, string? userSID, string? userEmail, string? userTelPhone, string? userCellPhone, bool isEnabled, string? userType, int? vendorId, string? chgUserName = null, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);

    /// <summary>
    /// 只做刪除標記，不做實際刪除資料
    /// </summary>
    /// <param name="users"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    bool DeleteUsers(IEnumerable<User> users, string? userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null);
}
