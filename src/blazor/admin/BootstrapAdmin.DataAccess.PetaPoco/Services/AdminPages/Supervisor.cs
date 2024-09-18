// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class SupervisorService : ISupervisor
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public SupervisorService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.Supervisor> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.Supervisor>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.Supervisor>> GetAllFrontAsync(SupervisorFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
            SELECT [Id]
                ,[Type]
                ,[OrganizationName]
                ,[Organizer]
                ,[Phone]
                ,[Email]
                ,[URL]
                ,[Remark]
                ,[ServiceContent]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [TeamOrganization]
                WHERE [IsDelete] = 0
                AND [Type] = 0
                AND [IsEnable] = 1
                  
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.OrganizationName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [OrganizationName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.OrganizationName}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] asc
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.Supervisor>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.Supervisor>> GetFrontAsync(SupervisorFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
             SELECT [Id]
                ,[Type]
                ,[OrganizationName]
                ,[Organizer]
                ,[Phone]
                ,[Email]
                ,[URL]
                ,[Remark]
                ,[ServiceContent]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [TeamOrganization]
                WHERE [IsDelete] = 0
                AND [Type] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.OrganizationName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [OrganizationName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.OrganizationName}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] asc
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.Supervisor>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.Supervisor>> GetAllAsync(SupervisorFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
             SELECT [Id]
                ,[Type]
                ,[OrganizationName]
                ,[Organizer]
                ,[Phone]
                ,[Email]
                ,[URL]
                ,[Remark]
                ,[ServiceContent]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [TeamOrganization]
                WHERE [IsDelete] = 0
                AND [Type] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.OrganizationName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [OrganizationName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.OrganizationName}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Phone] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Phone}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.URL))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [URL] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.URL}%");
            }

        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] asc
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.Supervisor>(sqlScript, [.. parameters]);
        return contracts;
    }
    public List<string>? GetWebNames(string Id)
    {
        using var db = DBManager.Create();
        var WebNames = db.Fetch<string>("SELECT [WebName] FROM [FileUploaded] WHERE [DataId] = @0", Id);

        if (WebNames is null)
            return WebNames;

        return WebNames.ToList();
    }

    public bool SaveWebName(string Id, string? WebName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.Supervisor>("SET [WebName]=CASE WHEN [WebName] IS NULL OR [WebName] ='' THEN '' ELSE [WebName] + ',' END +  @1 WHERE [Id] = @0", Id, WebName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteWebName(string Id, string? WebName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.Supervisor>("SET [WebName]=REPLACE(REPLACE([WebName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, WebName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
