// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using PetaPoco;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class GroupService : IGroup
{
    private IDBManager DBManager { get; }

    private IApLog AplogService { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="aplogService"></param>
    public GroupService(IDBManager db, IApLog aplogService)
        => (DBManager, AplogService) = (db, aplogService);
    
    public List<Group> GetAll()
    {
        using var db = DBManager.Create();
        string sqlScript = $"""
                SELECT [gro].*, ISNULL([dic].[Name], '') AS [GroupTypeName]
                FROM [Groups] AS [gro]
                LEFT JOIN [Dicts] AS [dic] ON [dic].[Category] = 'GroupType' AND [dic].[Code] = [gro].[GroupType]
                WHERE [gro].[IsDelete] = 0
            """;
        return db.Fetch<Group>(sqlScript);
    }

    public async Task<bool> DeleteGroups(IEnumerable<Group> groups, string? userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        bool ret;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            foreach (var group in groups)
            {
                string sqlScript = """
                        UPDATE [Groups]
                        SET [ParentId] = 0, [ModifyBy] = @1, [ModifyDate] = GETDATE()
                        WHERE [ParentID] = @0
                    """;
                db.Execute(sqlScript, group.Id, userName);

                sqlScript = """
                        UPDATE [Groups]
                        SET [IsDelete] = 1, [ModifyBy] = @1, [ModifyDate] = GETDATE()
                        WHERE [ID] = @0
                    """;
                var updRows = db.Execute(sqlScript, group.Id, userName);

                //apLog 2, 保存 log
                if (updRows >= 1)
                {
                    await AplogService.SaveChangesAsync(group, null, chgType, PGName, PGCode, userName, LoginIP);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByUserId(string? userId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select GroupID from UserGroup where UserID = @0", userId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByUserId(string? userId, IEnumerable<string> groupIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from UserGroup where UserID = @0", userId);
            db.InsertBatch("UserGroup", groupIds.Select(g => new { GroupID = g, UserID = userId }));
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public List<string> GetGroupsByRoleId(string? roleId)
    {
        using var db = DBManager.Create();
        return db.Fetch<string>("select GroupID from RoleGroup where RoleID = @0", roleId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="groupIds"></param>
    /// <returns></returns>
    public bool SaveGroupsByRoleId(string? roleId, IEnumerable<string> groupIds)
    {
        var ret = false;
        using var db = DBManager.Create();
        try
        {
            db.BeginTransaction();
            db.Execute("delete from RoleGroup where RoleID = @0", roleId);
            db.InsertBatch("RoleGroup", groupIds.Select(g => new { GroupID = g, RoleID = roleId }));
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

    /// <summary>
    /// 判斷是否有子節點
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public bool HasChildren(string? groupId)
    {
        var groups = GetAll();
        return groups.Any(g => g.ParentId == groupId);
    }
}
