// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Longbow.Security.Cryptography;
using Microsoft.Extensions.Localization;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class UserService : IUser
{
    private IDBManager DBManager { get; }

    private IStringLocalizer<UserService> Localizer { get; set; }

    private IApLog AplogService { set; get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="localizer"></param>
    /// <param name="aplogService"></param>
    public UserService(IDBManager db, IStringLocalizer<UserService> localizer, IApLog aplogService)
        => (DBManager, Localizer, AplogService) = (db, localizer, aplogService);

    public List<User> GetAll()
    {
        using var db = DBManager.Create();
        string sqlScript = $"""
                SELECT [user].*,
            	    ISNULL([dic].[Name], '') AS [UserTypeValue],
            	    REPLACE('(' + ISNULL([ven].[VendorNo], '') + ')' + ISNULL([ven].[VendorName], ''), '()', '') AS [VendorName]
                FROM [Users] AS [user]
                LEFT JOIN [Dicts] AS [dic]
                    ON [dic].[IsEnable] = 1
                        AND [dic].[IsDelete] = 0
                        AND [dic].[Category] = 'UserType'
            		    AND [dic].[Code] = [user].[UserType]
                LEFT JOIN [Vendors] AS [ven]
                    ON [ven].[IsDelete] = 0
                        AND [ven].[ID] = [user].[VendorID]
                WHERE [user].[IsDelete] = 0
            """;
        return db.Fetch<User>(sqlScript);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="authType">1, 登入驗證時(authType = Login), 要加上IsEnable 以及 IsDelete的判斷; 修改密碼時(authType = null), 暫不判斷 IsEnable 及IsDelete</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool Authenticate(string userName, string password, string? authType = null)
    {
        using var db = DBManager.Create();
        var user = db.SingleOrDefault<User>("select DisplayName, Password, PassSalt, IsEnable, IsDelete from Users where ApprovedTime is not null and UserName = @0", userName);

        var isAuth = false;
        if (user != null && !string.IsNullOrEmpty(user.PassSalt))
        {
            isAuth = user.Password == LgbCryptography.ComputeHash(password, user.PassSalt);
        }

        //登入驗證
        if (isAuth)
        {
            if ((authType ?? "") == "Login")
            {
                if (!user!.IsEnable || user.IsDelete)
                {
                    isAuth = false;
                }
            }
        }

        return isAuth;
    }

    public User? GetUserByUserName(string? userName)
    {
        using var db = DBManager.Create();
        //判斷時必然加上排除已經刪除的資料 IsDelete = 0
        return string.IsNullOrEmpty(userName) ? null : db.FirstOrDefault<User>("Where UserName = @0 and IsDelete = 0", userName);
    }

    public List<string> GetApps(string userName)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>($"select d.Code from Dicts d inner join RoleApp ra on d.Code = ra.AppId inner join (select r.Id from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 union select r.Id from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0) r on ra.RoleId = r.ID union select Code from Dicts where Category = @1 and exists(select r.ID from Roles r inner join UserRole ur on r.ID = ur.RoleID inner join Users u on ur.UserID = u.ID where u.UserName = @0 and r.RoleName = @2 union select r.ID from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID where u.UserName = @0 and r.RoleName = @2)", userName, "應用程式", "Administrators");
    }

    /// <summary>
    /// 通過使用者名獲得指定的前臺 AppId
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public string? GetAppIdByUserName(string userName)
    {
        using var db = DBManager.Create();
        return db.FirstOrDefault<User>("Where UserName = @0", userName)?.App;
    }

    public List<string> GetRoles(string userName)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>($"select r.RoleName from Roles r inner join UserRole ur on r.ID=ur.RoleID inner join Users u on ur.UserID = u.ID and u.UserName = @0 union select r.RoleName from Roles r inner join RoleGroup rg on r.ID = rg.RoleID inner join {db.Provider.EscapeSqlIdentifier("Groups")} g on rg.GroupID = g.ID inner join UserGroup ug on ug.GroupID = g.ID inner join Users u on ug.UserID = u.ID and u.UserName = @0", userName);
    }

    public List<string> GetUsersByGroupId(string? groupId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select UserID from UserGroup where GroupID = @0", groupId);
    }

    public bool SaveUsersByGroupId(string? id, IEnumerable<string> userIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserGroup where GroupId = @0", id);
            db.InsertBatch("UserGroup", userIds.Select(g => new { UserID = g, GroupID = id }));
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public List<string> GetUsersByRoleId(string? roleId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select UserID from UserRole where RoleID = @0", roleId);
    }

    public bool SaveUsersByRoleId(string? roleId, IEnumerable<string> userIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserRole where RoleID = @0", roleId);
            db.InsertBatch("UserRole", userIds.Select(g => new { UserID = g, RoleID = roleId }));
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool ChangePassword(string userName, string password, string newPassword)
    {
        var ret = false;
        using var db = DBManager.Create();
        if (Authenticate(userName, password))
        {
            var passSalt = LgbCryptography.GenerateSalt();
            password = LgbCryptography.ComputeHash(newPassword, passSalt);
            string sql = "set Password = @0, PassSalt = @1 where UserName = @2";
            ret = db.Update<User>(sql, password, passSalt, userName) == 1;
        }
        return ret;
    }
    public bool SaveDisplayName(string userName, string displayName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("set DisplayName = @1 where UserName = @0", userName, displayName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveTheme(string userName, string theme)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("set Css = @1 where UserName = @0", userName, theme) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveLogo(string userName, string? logo)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("set Icon = @1 where UserName = @0", userName, logo) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    /// <summary>
    /// 創建手機使用者
    /// </summary>
    /// <param name="phone"></param>
    /// <param name="code"></param>
    /// <param name="appId"></param>
    /// <param name="roles"></param>
    /// <returns></returns>
    public bool TryCreateUserByPhone(string phone, string code, string appId, ICollection<string> roles)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            var salt = LgbCryptography.GenerateSalt();
            var pwd = LgbCryptography.ComputeHash(code, salt);
            var user = db.FirstOrDefault<User>("Where UserName = @0", phone);
            if (user == null)
            {
                db.BeginTransaction();
                // 插入使用者
                user = new User()
                {
                    ApprovedBy = "Mobile",
                    ApprovedTime = DateTime.Now,
                    DisplayName = Localizer["MobileName"],
                    UserName = phone,
                    Icon = "default.jpg",
                    Description = Localizer["MobileDescription"],
                    PassSalt = salt,
                    Password = LgbCryptography.ComputeHash(code, salt),
                    App = appId
                };
                db.Save(user);
                // Authorization
                var roleIds = db.Fetch<string>("select ID from Roles where RoleName in (@roles)", new { roles });
                db.InsertBatch("UserRole", roleIds.Select(g => new { RoleID = g, UserID = user.Id }));
                db.CompleteTransaction();
            }
            else
            {
                user.PassSalt = salt;
                user.Password = pwd;
                db.Update(user);
            }
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveUser(string userName, string displayName, string password, int groupsId, string? userSID, string? userEmail, string? userTelPhone, string? userCellPhone, bool isEnabled, string? userType, int? vendorId, string? chgUserName = null, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        var salt = LgbCryptography.GenerateSalt();
        var pwd = LgbCryptography.ComputeHash(password, salt);
        using var db = DBManager.Create();
        var user = db.FirstOrDefault<User>("Where UserName = @0 and IsDelete = 0", userName);
        bool ret;
        if (user == null)
        {
            try
            {
                // 開始事務
                db.BeginTransaction();
                user = new User()
                {
                    ApprovedBy = "System",
                    ApprovedTime = DateTime.Now,
                    DisplayName = displayName, // Localizer["MobileName"], TODO 原來的手機使用者不對
                    UserName = userName,
                    Icon = "default.jpg",
                    Description = Localizer["MobileDefaultDescription"],
                    PassSalt = salt,
                    Password = pwd,
                    GroupsID = groupsId,
                    UserSID = userSID,
                    UserEmail = userEmail,
                    UserCellPhone = userCellPhone,
                    UserTelPhone = userTelPhone,
                    IsEnable = isEnabled,
                    UserType = userType,
                    VendorID = vendorId,
                };
                db.Save(user);

                //增加log
                AplogService.SaveChangesAsync(null, user, chgType, PGName, PGCode, chgUserName, LoginIP);

                // 授權 Default 角色
                db.Execute("insert into UserRole (UserID, RoleID) select ID, (select ID from Roles where RoleName = 'Default') RoleId from Users where UserName = @0", userName);
                // 結束事務
                db.CompleteTransaction();
                ret = true;
            }
            catch (Exception)
            {
                db.AbortTransaction();
                throw;
            }
        }
        else
        {
            //apLog 1, 更新前 model
            var oriUser = db.SingleOrDefault<User>("Where Id = @0", user.Id);

            user.DisplayName = displayName;
            //當修改時，輸入空密碼，則不更新原來的密碼
            user.PassSalt = string.IsNullOrEmpty(password) ? user.PassSalt : salt;
            user.Password = string.IsNullOrEmpty(password) ? user.Password : pwd;
            user.GroupsID = groupsId;
            user.UserSID = userSID;
            user.UserEmail = userEmail;
            user.UserCellPhone = userCellPhone;
            user.UserTelPhone = userTelPhone;
            user.IsEnable = isEnabled;
            user.UserType = userType;
            user.VendorID = vendorId;
            var updRows = db.Update(user);
            ret = true;

            //apLog 2, 保存 log
            if (updRows >= 1)
            {
                AplogService.SaveChangesAsync(oriUser, user, chgType, PGName, PGCode, chgUserName, LoginIP);
            }
        }
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool SaveApp(string userName, string app)
    {
        using var db = DBManager.Create();
        var ret = db.Update<User>("Set App = @1 Where UserName = @0", userName, app) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

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
    public bool DeleteUsers(IEnumerable<User> users, string? userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        if (users == null) return false;

        using var db = DBManager.Create();
        foreach (var user in users)
        {
            //為了保持UserName 的唯一性，在刪除時，對UserName暫時先加上標記 -Del20231101091209
            var ret = db.Update<User>("Set UserName = UserName + @3, IsDelete = @1, DeleteDate = Getdate(), DeleteBy = @2 Where ID = @0", user.Id, 1, userName, $"-Del{DateTime.Now.ToString("yyyyMMddHHmmss")}") == 1;

            //apLog , 保存 log
            if (ret)
            {
                AplogService.SaveChangesAsync(user, null, chgType, PGName, PGCode, userName, LoginIP);
            }
        }
        return true;
    }
}
