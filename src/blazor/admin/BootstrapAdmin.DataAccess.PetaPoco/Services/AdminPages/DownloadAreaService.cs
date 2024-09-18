// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class DownloadAreaService : IDownloadArea
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public DownloadAreaService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.DownloadArea> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.DownloadArea>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.DownloadArea>> GetAllFrontAsync(DownloadAreaFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT  [Id]
                ,[FileType]
                ,[FileName]
                ,[FilePath]
                ,[FileExtName]
                ,[FileDesc]
                ,[DataID]
                ,[StartDate]
                ,[EndDate]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [dbo].[RepairFiles]
            where (FileType = '5' or FileType = 'DownloadArea')
            and isdelete = 0
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [FileName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.FileName}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.DownloadArea>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.DownloadArea>> GetFrontAsync(DownloadAreaFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                 SELECT  [Id]
                ,[FileType]
                ,[FileName]
                ,[FilePath]
                ,[FileExtName]
                ,[FileDesc]
                ,[DataID]
                ,[StartDate]
                ,[EndDate]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [dbo].[RepairFiles]
            where ( FileType = '5' or FileType = 'DownloadArea')
            and [isdelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [FileName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.FileName}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.DownloadArea>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.DownloadArea>> GetAllAsync(DownloadAreaFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                 SELECT  [Id]
                ,[FileType]
                ,[FileName]
                ,[FilePath]
                ,[FileExtName]
                ,[FileDesc]
                ,[DataID]
                ,[StartDate]
                ,[EndDate]
                ,[IsEnable]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [dbo].[RepairFiles]
            where (FileType = '5' or FileType = 'DownloadArea')
            and isdelete = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.FileName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [FileName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.FileName}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.FileDesc))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [FileDesc] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.FileDesc}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.CreateBy))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [CreateBy] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.CreateBy}%");
            }

        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.DownloadArea>(sqlScript, [.. parameters]);
        return contracts;
    }
    public string GetFileNames(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [Id] = @0", Id);

        return fileNames.FirstOrDefault();

    }
    public string GetFilePath(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FilePath] FROM [RepairFiles] WHERE [Id] = @0", Id);

        return fileNames.FirstOrDefault();

    }

    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.DownloadArea>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.DownloadArea>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
