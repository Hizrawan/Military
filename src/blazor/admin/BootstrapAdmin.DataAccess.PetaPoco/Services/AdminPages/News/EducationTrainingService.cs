// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages.News;

internal class EducationTrainingService : IEducationTraining
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public EducationTrainingService(IDBManager db)
        => DBManager = db;

    public IEnumerable<EducationTraining> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<EducationTraining>().Where(b => !b.IsDelete);
    }

    public async Task<List<EducationTraining>> GetAllAsync(EducationTrainingFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT  [Id]
            ,[Author]
            ,[CourseTopic]
            ,[RegistrationStart]
            ,[RegistrationEnd]
            ,[CourseOutline]
            ,[IsPub]
            ,[Contact]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]
                FROM [EducationTrainings]
                WHERE [IsDelete] = 0

            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.CourseTopic))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [CourseTopic] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.CourseTopic}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.CourseOutline))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [CourseOutline] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.CourseOutline}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.State))
            {
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
                           AND [CreateDate] > GETDATE()
                    """;

                }
                if (filter.State == "3")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [CreateDate] < GETDATE()
                    """;

                }
                if (filter.State == "4")
                {
                    sqlScript = $"""
                        {sqlScript}
                          AND ([ispub] = 0 OR [ispub] =1)
                    """;

                }
                parameters.Add($"%{filter.State}%");
            }

            var isStartDateHasValue = filter.EducationTrainingStartDate.GetValueOrDefault() != default;
            var isEndDateHasValue = filter.EducationTrainingEndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) BETWEEN @{parameters.Count} AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.EducationTrainingStartDate.GetValueOrDefault());
                parameters.Add(filter.EducationTrainingEndDate.GetValueOrDefault());
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
                parameters.Add(filter.EducationTrainingStartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {
                // 無限期 ~ ContractEndDate
                // 資料的起始日和終止日需要小於 ContractEndDate
                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (CAST ([CreateDate] AS DATE) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.EducationTrainingEndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                        {sqlScript}
                           ORDER BY [CreateDate] DESC;
                    """;

        }
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.News.EducationTraining>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<EducationTraining>> GetAllFrontAsync(EducationTrainingFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
                SELECT  [Id]
            ,[Author]
            ,[CourseTopic]
            ,[RegistrationStart]
            ,[RegistrationEnd]
            ,[CourseOutline]
            ,[IsPub]
            ,[Contact]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]
                FROM [EducationTrainings]
                WHERE [IsDelete] = 0
                AND [IsPub] = 1

            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.CourseTopic))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [CourseTopic] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.CourseTopic}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.CourseOutline))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [CourseOutline] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.CourseOutline}%");
            }

            sqlScript = $"""
                        {sqlScript}
                           ORDER BY [CreateDate] DESC;
                    """;

        }
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.News.EducationTraining>(sqlScript, [.. parameters]);
        return contracts;
    }
}

