// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class LeaveMessageService : ILeaveMessage
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public LeaveMessageService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.LeaveMessage> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.LeaveMessage>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.LeaveMessage>> GetAllFrontAsync(LeaveMessageFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT [Id]
                ,[MessageTitle]
                ,[Sender]
                ,[ContactNumber]
                ,[ReleaseDate]
                ,[Email]
                ,[Content]
                ,[IsProcessed]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [LeaveMessage]
                WHERE [IsDelete] = 0
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.MessageTitle))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [MessageTitle] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.MessageTitle}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.LeaveMessage>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.LeaveMessage>> GetFrontAsync(LeaveMessageFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT [Id]
                ,[MessageTitle]
                ,[Sender]
                ,[ContactNumber]
                ,[ReleaseDate]
                ,[Email]
                ,[Content]
                ,[IsProcessed]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [LeaveMessage]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.MessageTitle))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [MessageTitle] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.MessageTitle}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.LeaveMessage>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.LeaveMessage>> GetAllAsync(LeaveMessageFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
             SELECT [Id]
                ,[MessageTitle]
                ,[Sender]
                ,[ContactNumber]
                ,[ReleaseDate]
                ,[Email]
                ,[Content]
                ,[IsProcessed]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [LeaveMessage]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.MessageTitle))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND ([MessageTitle] LIKE @{parameters.Count} OR [Sender] LIKE @{parameters.Count} OR [ContactNumber] LIKE @{parameters.Count})
                    """;
                parameters.Add($"%{filter.MessageTitle}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.IsProcessed))
            {
                if (filter.IsProcessed == "0")
                {
                    sqlScript = $"""
                        {sqlScript}
                            AND [IsProcessed] = 0
                    """;
                    parameters.Add($"%{filter.IsProcessed}%");
                }
                else
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [IsProcessed] = 1
                    """;
                    parameters.Add($"%{filter.IsProcessed}%");
                }

            }


        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.LeaveMessage>(sqlScript, [.. parameters]);
        return contracts;
    }
    public List<string>? GetFileNames(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [FileUploaded] WHERE [DataId] = @0", Id);

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }

    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.LeaveMessage>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.LeaveMessage>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
