// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class LinkService : ILink
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public LinkService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.Link> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.Link>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.Link>> GetAllFrontAsync(LinkFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
             SELECT [Id]
                ,[WebName]
                ,[WebLink]
                ,[UnitId]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [dbo].[Links]
                WHERE [IsDelete] = 0
                AND [IsEnable] = 1
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.WebName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [WebName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.WebName}%");
            }

            //sqlScript = $"""
            //    {sqlScript}
            //        ORDER BY [CreateDate] DESC
            //    """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.Link>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.Link>> GetFrontAsync(LinkFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
              SELECT [Id]
                ,[WebName]
                ,[WebLink]
                ,[UnitId]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [dbo].[Links]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.WebName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [WebName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.WebName}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.Link>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.Link>> GetAllAsync(LinkFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT [Id]
                ,[WebName]
                ,[WebLink]
                ,[UnitId]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [dbo].[Links]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.WebName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [WebName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.WebName}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.WebLink))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [WebLink] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.WebLink}%");
            }

        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.Link>(sqlScript, [.. parameters]);
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
        var ret = db.Update<DataAccess.Models.AdminPages.Link>("SET [WebName]=CASE WHEN [WebName] IS NULL OR [WebName] ='' THEN '' ELSE [WebName] + ',' END +  @1 WHERE [Id] = @0", Id, WebName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteWebName(string Id, string? WebName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.Link>("SET [WebName]=REPLACE(REPLACE([WebName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, WebName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
