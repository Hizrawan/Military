// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

/// <summary>
/// 
/// </summary>
internal class NavigationService : INavigation
{
    private IDBManager DBManager { get; }

    private IApLog AplogService { set; get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="aplogService"></param>
    public NavigationService(IDBManager db, IApLog aplogService)
        => (DBManager, AplogService) = (db, aplogService);

    /// <summary>
    /// 獲得指定使用者名可訪問的所有功能表集合
    /// </summary>
    /// <param name="userName">當前使用者名</param>
    /// <returns>未層次化的功能表集合</returns>
    public List<Navigation> GetAllMenus(string userName)
    {
        using var db = DBManager.Create();
        // 加上判斷條件為 IsDelete = 0
        string sqlScript = """
                SELECT [navi].[ID], [navi].[ParentId], [navi].[Name], [navi].[Order], ISNULL([navi].[Icon], '') AS Icon,
                    ISNULL([navi].[Url], '') AS Url, [navi].[Category], [navi].[Target], [navi].[IsResource], [navi].[Application],
                    [navi].[AppSort], [navi].[IsEnable]
                FROM [Navigations] AS [navi]
                JOIN (
                        SELECT [nr].[NavigationID]
                        FROM [Users] AS [user]
                        JOIN [UserRole] AS [ur] ON [ur].[UserID] = [user].[ID]
                        JOIN [NavigationRole] AS [nr] ON [nr].[RoleID] = [ur].[RoleID]
                        JOIN [Roles] AS [role] ON [role].[ID] = [ur].[RoleID]
                        WHERE [user].[UserName] = @UserName  And [role].[IsEnable] = 1 And [role].[IsDelete] = 0
                    UNION
                        SELECT [nr].[NavigationID]
                        FROM [Users] AS [user]
                        JOIN [RoleGroup] AS [rg] ON [rg].[GroupID] = [user].[GroupsID]
                        JOIN [NavigationRole] AS [nr] ON [nr].[RoleID] = [rg].[RoleID]
                        WHERE [user].[UserName] = @UserName And [user].[IsEnable] = 1 And [user].[IsDelete] = 0
                    UNION
                        SELECT [nr].[NavigationID]
                        FROM [Users] AS [user]
                        JOIN [UserGroup] AS [ug] ON [ug].[UserID] = [user].[ID]
                        JOIN [RoleGroup] AS [rg] ON [rg].[GroupID] = [ug].[GroupID]
                        JOIN [NavigationRole] AS [nr] ON [nr].[RoleID] = [rg].[RoleID]
                        WHERE [user].[UserName] = @UserName And [user].[IsEnable] = 1 And [user].[IsDelete] = 0
                    UNION
                        SELECT [navi].[ID]
                        FROM [Navigations] AS [navi]
                        WHERE EXISTS (
                            SELECT [UserName]
                            FROM [Users] AS [user]
                            JOIN [UserRole] AS [ur] ON [ur].[UserID] = [user].[ID]
                            JOIN [Roles] AS [role] ON [role].[ID] = [ur].[RoleID]
                            WHERE [user].[UserName] = @UserName
                                AND [role].[RoleName] = @RoleName And [user].[IsEnable] = 1 And [user].[IsDelete] = 0 And [role].[IsEnable] = 1 And [role].[IsDelete] = 0 
                        )
                ) AS [subNavi] ON [subNavi].[NavigationID] = [navi].[ID]
                WHERE [navi].[IsDelete] = 0
                ORDER BY [navi].[Application], [navi].[AppSort], [navi].[Order]
            """;

        return db.Fetch<Navigation>(sqlScript, new { UserName = userName, RoleName = "Administrators" });
    }

    public List<string> GetMenusByRoleId(string? roleId)
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT [NavigationID]
                FROM [NavigationRole]
                WHERE [RoleID] = @RoleId
            """;
        return db.Fetch<string>(sqlScript, new { RoleId = roleId });
    }

    public Navigation? GetMenuByUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        using var db = DBManager.Create();
        string sqlScript = """
                SELECT *
                FROM [Navigations]
                WHERE CharIndex(@0, [Url]) >= 1
            """;
        return db.FirstOrDefault<Navigation>(sqlScript, url);
    }

    public bool SaveMenusByRoleId(string? roleId, List<string> menuIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("DELETE FROM [NavigationRole] WHERE [RoleID] = @RoleId", new { RoleId = roleId });
            db.InsertBatch("NavigationRole", menuIds.Select(g => new { NavigationID = g, RoleID = roleId }));
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

    public async Task<bool> DeleteMenus(IEnumerable<Navigation> menus, string? userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        bool ret;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            foreach (var menu in menus)
            {
                string sqlScript = """
                        UPDATE [Navigations]
                        SET [ParentId] = 0, [ModifyBy] = @1, [ModifyDate] = GETDATE()
                        WHERE [ParentId] = @0
                    """;
                db.Execute(sqlScript, menu.Id, userName);

                sqlScript = """
                        UPDATE [Navigations]
                        SET [IsDelete] = 1, [ModifyBy] = @1, [ModifyDate] = GETDATE()
                        WHERE [ID] = @0
                    """;
                var updRows = db.Execute(sqlScript, menu.Id, userName);

                //apLog 2, 保存 log
                if (updRows >= 1)
                {
                    await AplogService.SaveChangesAsync(menu, null, chgType, PGName, PGCode, userName, LoginIP);
                }
            }

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
}
