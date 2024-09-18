// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages.PermissionControl;

internal class UnitMaintenanceService : IUnitMaintenance
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public UnitMaintenanceService(IDBManager db)
        => DBManager = db;

    public IEnumerable<UnitMaintenance> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<UnitMaintenance>().Where(b => !b.IsDelete);
    }
    public async Task<List<UnitMaintenance>> GetAllFrontAsync(UnitMaintenanceFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id]
            ,[UnitName]
            ,[Email]
            ,[Address]
            ,[Phone]
            ,[Fax]
            ,[Code]
            ,[UnitSupervisor]
            ,[OldCode]
            ,[StartIP]
            ,[EndIP]
            ,[IP]
            ,[BusinessContent]
            ,[Allocation]
            ,[UnitOrder]
            ,[UnitSort]
            ,[IsEnable]
            ,[OtherInfo]
            ,[Ext]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]

                FROM [Unit]
                WHERE [IsDelete] = 0
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.UnitName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [UnitName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.UnitName}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<UnitMaintenance>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<UnitMaintenance>> GetFrontAsync(UnitMaintenanceFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT [Id]
            ,[UnitName]
            ,[Email]
            ,[Address]
            ,[Phone]
            ,[Fax]
            ,[Code]
            ,[UnitSupervisor]
            ,[OldCode]
            ,[StartIP]
            ,[EndIP]
            ,[IP]
            ,[BusinessContent]
            ,[Allocation]
            ,[UnitOrder]
            ,[UnitSort]
            ,[IsEnable]
            ,[OtherInfo]
            ,[Ext]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]

                 FROM [Unit]
                WHERE [IsDelete] = 0
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.UnitName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [UnitName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.UnitName}%");
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<UnitMaintenance>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<UnitMaintenance>> GetAllAsync(UnitMaintenanceFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id]
            ,[UnitName]
            ,[Email]
            ,[Address]
            ,[Phone]
            ,[Fax]
            ,[Code]
            ,[UnitSupervisor]
            ,[OldCode]
            ,[StartIP]
            ,[EndIP]
            ,[IP]
            ,[BusinessContent]
            ,[Allocation]
            ,[UnitOrder]
            ,[UnitSort]
            ,[IsEnable]
            ,[OtherInfo]
            ,[Ext]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]

                FROM  [Unit]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.UnitName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [UnitName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.UnitName}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.UnitSupervisor))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [UnitSupervisor] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.UnitSupervisor}%");
            }

        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<UnitMaintenance>(sqlScript, [.. parameters]);
        return contracts;
    }
    public List<string>? GetFileNames(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [DataId] = @0 AND [FileType] = @1", Id, "NB");

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }

    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<UnitMaintenance>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<UnitMaintenance>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
