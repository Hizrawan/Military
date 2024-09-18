// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class RoleService : IRole
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public RoleService(IDBManager db)
        => DBManager = db;

    public List<Role> GetAll()
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT [rol].*
                FROM [Roles] AS [rol]
                WHERE [rol].[IsDelete] = 0
            """;
        return db.Fetch<Role>(sqlScript);
    }

    public List<string> GetRolesByGroupId(string? groupId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select RoleID from RoleGroup where GroupID = @0", groupId);
    }

    public List<string> GetRolesByUserId(string? userId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select RoleID from UserRole where UserID = @0", userId);
    }

    public List<string> GetRolesByMenuId(string? menuId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select RoleID from NavigationRole where NavigationID = @0", menuId);
    }

    public bool SaveRolesByGroupId(string? groupId, IEnumerable<string> roleIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from RoleGroup where GroupID = @0", groupId);
            db.InsertBatch("RoleGroup", roleIds.Select(g => new { RoleID = g, GroupID = groupId }));
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
    
    public bool SaveRolesByUserId(string? userId, IEnumerable<string> roleIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserRole where UserID = @0", userId);
            db.InsertBatch("UserRole", roleIds.Select(g => new { RoleID = g, UserID = userId }));
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

    public bool SaveRolesByMenuId(string? menuId, IEnumerable<string> roleIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from NavigationRole where NavigationID = @0", menuId);
            db.InsertBatch("NavigationRole", roleIds.Select(g => new { RoleID = g, NavigationID = menuId }));
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

    public bool DeleteRoles(IEnumerable<Role> roles, string? userName)
    {
        bool ret;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            foreach (var role in roles)
            {
                string sqlScript = """
                    UPDATE [Roles]
                    SET [IsDelete] = 1, [ModifyBy] = @0, [ModifyDate] = GETDATE()
                    WHERE ID = @1
                """;
                db.Execute(sqlScript, userName, role.Id);
            }

            // TODO activity log

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
