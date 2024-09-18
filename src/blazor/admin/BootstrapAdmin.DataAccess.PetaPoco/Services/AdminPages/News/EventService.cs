// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages.News;

internal class EventService : IEvent
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public EventService(IDBManager db)
        => DBManager = db;

    public IEnumerable<Event> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<Event>().Where(b => !b.IsDelete);
    }
    public async Task<List<Event>> GetAllFrontAsync(EventFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT  [Id],
                        [Title],
                        [Content],
                        [StartDate],
                        [EndDate],
                        [OrdinaryMember],
                        [HonoraryMember],
                        [CrossBorderMember],
                        [EventCategory],
                        [IsPub],
                        [IsShow],
                        [IsDelete],
                        [CreateBy],
                        [CreateDate],
                        [ModifyBy],
                        [ModifyDate],
                        [DeleteBy],
                        [DeleteDate]

                FROM [Events]
                WHERE [IsDelete] = 0
             AND [IsShow] = 1
                    AND [IsPub]=1
                    AND [EventCategory] = 1
                    AND (
                            ( GETDATE() > [StartDate]
                                AND GETDATE() < [EndDate] )
                            OR ( [StartDate] IS NULL
                                AND [EndDate] IS NULL )
                            OR ( GETDATE() > [StartDate]
                                AND [EndDate] IS NULL )
                            OR ( [StartDate] IS NULL
                                AND GETDATE() < [EndDate] )
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

            var isStartDateHasValue = filter.EventStartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EventEndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND (CAST([CreateDate] AS DATE) BETWEEN @{parameters.Count} AND @{parameters.Count + 1})
                    """;
                parameters.Add(filter.EventStartDate.GetValueOrDefault());
                parameters.Add(filter.EventEndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND  (CAST ([CreateDate] AS DATE) >= @{parameters.Count})
                    """;
                parameters.Add(filter.EventStartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {
                // 無限期 ~ ContractEndDate
                // 資料的起始日和終止日需要小於 ContractEndDate
                sqlScript = $"""
                        {sqlScript}
                            AND (CAST([CreateDate] AS DATE) <= @{parameters.Count})
                    """;
                parameters.Add(filter.EventEndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }
        #endregion
        var contracts = await db.FetchAsync<Event>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<Event>> GetFrontAsync(EventFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT  [Id],
                        [Title],
                        [Content],
                        [StartDate],
                        [EndDate],
                        [OrdinaryMember],
                        [HonoraryMember],
                        [CrossBorderMember],
                        [EventCategory],
                        [IsPub],
                        [IsShow],
                        [IsDelete],
                        [CreateBy],
                        [CreateDate],
                        [ModifyBy],
                        [ModifyDate],
                        [DeleteBy],
                        [DeleteDate]

                FROM [Events]
                WHERE [IsDelete] = 0
                    AND [IsShow] = 1
                    AND [IsPub]=1
                    AND [EventCategory] = 1
                    AND (
                            ( GETDATE() > [StartDate]
                            AND GETDATE() < [EndDate] )
                            OR ( [StartDate] IS NULL
                            AND [EndDate] IS NULL )
                            OR ( GETDATE() > [StartDate]
                            AND [EndDate] IS NULL )
                            OR ( [StartDate] IS NULL
                            AND GETDATE() < [EndDate] )
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

            var isStartDateHasValue = filter.EventStartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EventEndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND (CAST([CreateDate] AS DATE)
                                BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                    """;
                parameters.Add(filter.EventStartDate.GetValueOrDefault());
                parameters.Add(filter.EventEndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND  (CAST ([CreateDate] AS DATE) >= @{parameters.Count})
                    """;
                parameters.Add(filter.EventStartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {
                // 無限期 ~ ContractEndDate
                // 資料的起始日和終止日需要小於 ContractEndDate
                sqlScript = $"""
                        {sqlScript}
                            AND (CAST([CreateDate] AS DATE) <= @{parameters.Count})
                    """;
                parameters.Add(filter.EventEndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }
        #endregion
        var contracts = await db.FetchAsync<Event>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<Event>> GetAllAsync(EventFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT  [Id],
                        [Title],
                        [Content],
                        [StartDate],
                        [EndDate],
                        [OrdinaryMember],
                        [HonoraryMember],
                        [CrossBorderMember],
                        [EventCategory],
                        [IsPub],
                        [IsShow],
                        [IsDelete],
                        [CreateBy],
                        [CreateDate],
                        [ModifyBy],
                        [ModifyDate],
                        [DeleteBy],
                        [DeleteDate]

                 FROM [Events]

                WHERE [IsDelete] = 0
                    AND [EventCategory] = 1
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
            if (filter.State == "0")
            {
                sqlScript = $"""
                        {sqlScript}
                            AND ([isPub] = 0 OR [ispub] = 1)
                    """;

            }
            if (filter.State == "1")
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [isPub] = 0
                    """;

            }
            if (filter.State == "2")
            {
                sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                    """;

            }
            parameters.Add($"%{filter.State}%");




            var isStartDateHasValue = filter.EventStartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EventEndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST([CreateDate] AS DATE)
                                    BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.EventStartDate.GetValueOrDefault());
                parameters.Add(filter.EventEndDate.GetValueOrDefault());
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
                parameters.Add(filter.EventStartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {
                // 無限期 ~ ContractEndDate
                // 資料的起始日和終止日需要小於 ContractEndDate
                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EventEndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<Event>(sqlScript, [.. parameters]);
        return contracts;
    }

    //public async Task<List<EventCarousel>> GetPaths()
    //{
    //    using var db = DBManager.Create();
    //    var sqlScript = """
    //            SELECT  RF.[Id],
    //                    RF.[FileType],
    //                    RF.[FileName],
    //                    RF.[FilePath],
    //                    RF.[FileExtName],
    //                    RF.[FileDesc],
    //                    RF.[DataID],
    //                    RF.[IsDelete],
    //         	        IC.[Sort],
    //         	        IC.[CarouselType],
    //                    RF.[CreateBy],
    //                    RF.[CreateDate],
    //                    RF.[ModifyBy],
    //                    RF.[ModifyDate],
    //                    RF.[DeleteBy],
    //                    RF.[DeleteDate],
    //         	        [FilePath]+[FileName] AS [path]

    //            FROM [RepairFiles] AS RF
    //                 JOIN [dbo].[ImageCarousels] AS IC
    //                   ON IC.Id = RF.[DataID]

    //            WHERE RF.[isdelete]=0
    //              AND IC.[IsEnable]=1
    //              AND RF.[FileType] = 'IC'

    //        """;

    //    var contracts = await db.FetchAsync<EventCarousel>(sqlScript);
    //    return contracts;

    //}

    public List<string>? GetFileNames(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("SELECT [FileName] FROM [RepairFiles] WHERE [DataId] = @0 AND [FileType] = @1", Id, "AI");

        if (fileNames is null)
            return fileNames;

        return fileNames.ToList();
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

    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<Event>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<Event>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
