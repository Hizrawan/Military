// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class SWUService : ISWU
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public SWUService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.SWU> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.SWU>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.SWU>> GetAllFrontAsync(SWUFilter? filter = null)
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
                AND [Type] = 1
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
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.SWU>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.SWU>> GetFrontAsync(SWUFilter? filter = null)
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
                AND [Type]=1
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
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.SWU>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.SWU>> GetAllAsync(SWUFilter? filter = null)
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
                AND [Type]=1
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
            if (!string.IsNullOrWhiteSpace(filter.URL))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [URL] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.URL}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Phone] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Phone}%");
            }
        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.SWU>(sqlScript, [.. parameters]);
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
        var ret = db.Update<DataAccess.Models.AdminPages.SWU>("SET [WebName]=CASE WHEN [WebName] IS NULL OR [WebName] ='' THEN '' ELSE [WebName] + ',' END +  @1 WHERE [Id] = @0", Id, WebName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteWebName(string Id, string? WebName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.SWU>("SET [WebName]=REPLACE(REPLACE([WebName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, WebName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
