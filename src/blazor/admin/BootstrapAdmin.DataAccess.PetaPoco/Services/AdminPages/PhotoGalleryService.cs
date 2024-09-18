// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class PhotoGalleryService : IPhotoGallery
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public PhotoGalleryService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.PhotoGallery> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.PhotoGallery>().Where(b => !b.IsDelete);
    }
    public List<string>? GetTitles(string Id)
    {
        using var db = DBManager.Create();
        var Titles = db.Fetch<string>("SELECT [Title] FROM [RepairFiles] WHERE [DataId] = @0 AND [FileType] = @1", Id, "PhotoGallery");

        if (Titles is null)
        {
            return Titles;
        }
        return Titles.ToList();
    }
    public async Task<List<DataAccess.Models.AdminPages.PhotoGallery>> GetAllFrontAsync(PhotoGalleryFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
            SELECT [Id]
                ,[Title]
                ,[Content]
                ,[NewsType]
                ,[StartDate]
                ,[EndDate]
                ,[IsPub]
                ,[IsShow]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [VSPC].[dbo].[Bulletins]
            where newstype =3

            and isdelete =0
            AND (
               (GETDATE()>[StartDate] AND GETDATE()<[EndDate])
               OR ([StartDate] IS NULL AND [EndDate] IS NULL)
               OR (GETDATE() > [StartDate] AND [EndDate] IS NULL)
               OR ([StartDate] IS NULL AND GETDATE()<[EndDate])
            )
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Title] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Title}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.PhotoGallery>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.PhotoGallery>> GetFrontAsync(PhotoGalleryFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
             SELECT  [Id]
                ,[Title]
                ,[Content]
                ,[NewsType]
                ,[StartDate]
                ,[EndDate]
                ,[IsPub]
                ,[IsShow]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [VSPC].[dbo].[Bulletins]
            where newstype =3

            and isdelete =0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Title] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Title}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.PhotoGallery>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.PhotoGallery>> GetAllAsync(PhotoGalleryFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
            SELECT [Id]
                ,[Title]
                ,[Content]
                ,[NewsType]
                ,[StartDate]
                ,[EndDate]
                ,[IsPub]
                ,[IsShow]
                ,[IsDelete]
                ,[CreateBy]
                ,[CreateDate]
                ,[ModifyBy]
                ,[ModifyDate]
                ,[DeleteBy]
                ,[DeleteDate]
            FROM [VSPC].[dbo].[Bulletins]
            where newstype =3

            and isdelete =0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Title] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Title}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Content))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Content] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Content}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.State))
            {
                if (filter.State == "0")
                {
                    sqlScript = $"""
                        {sqlScript}
                            
                    """;

                }
                if (filter.State == "1")
                {
                    sqlScript = $"""
                        {sqlScript}
                            AND (
                       (GETDATE()<[StartDate] OR GETDATE()>[EndDate])
                    )
                    """;

                }
                if (filter.State == "2")
                {
                    sqlScript = $"""
                        {sqlScript}
                         AND (
                       (GETDATE()>[StartDate] AND GETDATE()<[EndDate])
                       OR ([StartDate] IS NULL AND [EndDate] IS NULL)
                       OR (GETDATE() > [StartDate] AND [EndDate] IS NULL)
                       OR ([StartDate] IS NULL AND GETDATE()<[EndDate])
                    )
                    """;

                }
                parameters.Add($"%{filter.State}%");
            }

        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.PhotoGallery>(sqlScript, [.. parameters]);
        return contracts;
    }


    public bool SaveTitle(string Id, string? Title)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.PhotoGallery>("SET [Title]=CASE WHEN [Title] IS NULL OR [Title] ='' THEN '' ELSE [Title] + ',' END +  @1 WHERE [Id] = @0", Id, Title) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteTitle(string Id, string? Title)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.PhotoGallery>("SET [Title]=REPLACE(REPLACE([Title], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, Title) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public List<string>? GetFileNames(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [DataId] = @0", Id);

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }

    public List<string>? GetFilePath(string Id, string fileType)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FilePath] FROM [RepairFiles] WHERE [DataId] = @0 AND [FileType] = @1", Id, fileType);

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }

    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.PhotoGallery>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.PhotoGallery>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
