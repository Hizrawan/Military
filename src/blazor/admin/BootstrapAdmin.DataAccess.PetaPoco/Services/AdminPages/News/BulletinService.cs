// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages.News;

internal class BulletinService : IBulletin
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public BulletinService(IDBManager db)
        => DBManager = db;

    public IEnumerable<Bulletin> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<Bulletin>().Where(b => !b.IsDelete);
    }
    public async Task<List<Bulletin>> GetAllFrontAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                FROM [Bulletins]
                WHERE [IsDelete] = 0
             AND [NewsType] = 0
            AND [IsPub]=1
               
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



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE)
                                    BETWEEN CAST(@{parameters.Count} AS DATE)
                                    AND CAST(@{parameters.Count + 1} AS DATE))
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                               (CAST([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<Bulletin>> GetFrontAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                 FROM [Bulletins]
                WHERE [IsDelete] = 0
                AND [NewsType] = 0
                    AND [IsShow]=1
                    AND [IsPub]=1
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



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) BETWEEN CAST(@{parameters.Count} AS DATE) AND CAST(@{parameters.Count + 1} AS DATE))
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                               (CAST([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<Bulletin> getimagedetail(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                 FROM [Bulletins]
                WHERE [IsDelete] = 0
                AND [Id] = 772
                AND [NewsType] = 0
                    AND [IsShow]=1
                    AND [IsPub]=1
                    AND (
                            (GETDATE()>[StartDate] AND GETDATE()<[EndDate])
                            OR ([StartDate] IS NULL AND [EndDate] IS NULL)
                            OR (GETDATE() > [StartDate] AND [EndDate] IS NULL)
                            OR ([StartDate] IS NULL AND GETDATE()<[EndDate])
                         )
            """;


        var imagedetail = await db.FetchAsync<Bulletin>(sqlScript);
        return imagedetail.FirstOrDefault();
    }
    public async Task<List<Bulletin>> GetFrontEventAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                 FROM [Bulletins]
                WHERE [IsDelete] = 0
                AND [NewsType]=1
                    AND [IsShow]=1
                    AND [IsPub]=1
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



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) BETWEEN CAST(@{parameters.Count} AS DATE) AND CAST(@{parameters.Count + 1} AS DATE))
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                               (CAST([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<Bulletin>> GetFrontEduAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                 FROM [Bulletins]
                WHERE [IsDelete] = 0
                AND [NewsType]=2
                    AND [IsShow]=1
                    AND [IsPub]=1
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



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) BETWEEN CAST(@{parameters.Count} AS DATE) AND CAST(@{parameters.Count + 1} AS DATE))
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                               (CAST([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<Bulletin>> GetAllAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsShow],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                FROM  [Bulletins]
                WHERE [IsDelete] = 0
            AND [NewsType]=0
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
                            AND ([isPub] = 0 OR [isPub] = 1)
                    """;

                }
                if (filter.State == "1")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] > GETDATE()
                    """;

                }
                if (filter.State == "2")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] < GETDATE()
                    """;
                }
                parameters.Add($"%{filter.State}%");
            }



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE)
                                    BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<Bulletin>> GetAllEventAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsShow],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                FROM  [Bulletins]
                WHERE [IsDelete] = 0
             AND [NewsType]=1
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
                            AND ([isPub] = 0 OR [isPub] = 1)
                    """;

                }
                if (filter.State == "1")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] > GETDATE()
                    """;

                }
                if (filter.State == "2")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] < GETDATE()
                    """;
                }
                parameters.Add($"%{filter.State}%");
            }



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE)
                                    BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<Bulletin>> GetAllEduAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsShow],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                FROM  [Bulletins]
                WHERE [IsDelete] = 0
                AND [NewsType]=2
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
                            AND ([isPub] = 0 OR [isPub] = 1)
                    """;

                }
                if (filter.State == "1")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] > GETDATE()
                    """;

                }
                if (filter.State == "2")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] < GETDATE()
                    """;
                }
                parameters.Add($"%{filter.State}%");
            }



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE)
                                    BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<Bulletin>> GetAllPhotoAsync(BulletinFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT  [Id],
                       [Title],
                       [Content],
                       [StartDate],
                       [EndDate],
                       [IsPub],
                       [IsShow],
                       [IsDelete],
                       [CreateBy],
                       [CreateDate],
                       [ModifyBy],
                       [ModifyDate],
                       [DeleteBy],
                       [DeleteDate],[NewsType]

                FROM  [Bulletins]
                WHERE [IsDelete] = 0
                AND [NewsType]=3
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
                            AND ([isPub] = 0 OR [isPub] = 1)
                    """;

                }
                if (filter.State == "1")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] > GETDATE()
                    """;

                }
                if (filter.State == "2")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] < GETDATE()
                    """;
                }
                parameters.Add($"%{filter.State}%");
            }



            var isStartDateHasValue = filter.StartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE)
                                    BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.StartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EndDate.GetValueOrDefault());
            }
        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<Bulletin>(sqlScript, [.. parameters]);
        return contracts;
    }
    public List<string>? GetFileNames(string Id, string fileType, string other)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [DataId] = @0 AND ([FileType] = @1 OR [FileType] = @2)", Id, fileType, other);

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }

    public List<string>? GetFilePath(string Id, string fileType, string other)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FilePath] FROM [RepairFiles] WHERE [DataId] = @0 AND ([FileType] = @1 OR [FileType] = @2)  order by CreateDate", Id, fileType, other);

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }

    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<Bulletin>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<Bulletin>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public List<string>? GetFileNamesHighlight(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [DataId] = @0 AND [FileType] = @1", Id, "SC");

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }
    public List<string>? GetFileNamesNews(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [DataId] = @0 AND [FileType] = @1", Id, "NB");

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
    }



}
