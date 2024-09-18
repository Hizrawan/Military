// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using System;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class VolunteerRegistrationService : IVolunteerRegistration
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public VolunteerRegistrationService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.VolunteerRegistration> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.VolunteerRegistration>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.VolunteerRegistration>> GetAllFrontAsync(VolunteerRegistrationFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
              SELECT [Id]
            ,[Name]
            ,[IdNumber]
            ,[Gender]
            ,[Phone]
            ,[Mobile]
            ,[Email]
            ,[Degree]
            ,[Occupation]
            ,[SchoolName]
            ,[Speciality]
            ,[DateofBirth]
            ,[ServiceType]
            ,[AbleFixed]
            ,[NotAbleFixed]
            ,[Flexible]
            ,[Remarks]
            ,[HaveRecord]
            ,[IsEnable]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]
             FROM [VolunteerRegistration]

                WHERE [IsDelete] = 0
                   
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Name] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Name}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.VolunteerRegistration>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.VolunteerRegistration>> GetFrontAsync(VolunteerRegistrationFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
               SELECT [Id]
            ,[Name]
            ,[IdNumber]
            ,[Gender]
            ,[Phone]
            ,[Mobile]
            ,[Email]
            ,[Degree]
            ,[Occupation]
            ,[SchoolName]
            ,[Speciality]
            ,[DateofBirth]
            ,[ServiceType]
            ,[AbleFixed]
            ,[NotAbleFixed]
            ,[Flexible]
            ,[Remarks]
            ,[HaveRecord]
            ,[IsEnable]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]
             FROM [VolunteerRegistration]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Name] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Name}%");
            }

            sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        }

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.VolunteerRegistration>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.VolunteerRegistration>> GetAllAsync(VolunteerRegistrationFilter? filter = null)
    {



        using var db = DBManager.Create();
        var sqlScript = """
             SELECT [Id]
            ,[Name]
            ,[IdNumber]
            ,[Gender]
            ,[Phone]
            ,[Mobile]
            ,[Email]
            ,[Degree]
            ,[Occupation]
            ,[SchoolName]
            ,[Speciality]
            ,[DateofBirth]
            ,[ServiceType]
            ,[AbleFixed]
            ,[NotAbleFixed]
            ,[Flexible]
            ,[Remarks]
            ,[HaveRecord]
            ,[IsEnable]
            ,[IsDelete]
            ,[CreateBy]
            ,[CreateDate]
            ,[ModifyBy]
            ,[ModifyDate]
            ,[DeleteBy]
            ,[DeleteDate]
             FROM [VolunteerRegistration]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND ([Name] LIKE @{parameters.Count} OR YEAR([DateofBirth]) LIKE @{parameters.Count})
                    """;
                parameters.Add($"%{filter.Name}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Gender))
            {
                if (filter.Gender == "3")
                {
                    sqlScript = $"""
                        {sqlScript}
                            AND( [Gender] = '2' or [Gender] = '1')
                    """;
                    parameters.Add($"{filter.Gender}");
                }
                else
                {
                    sqlScript = $"""
                        {sqlScript}
                            AND [Gender] = @{parameters.Count}
                    """;
                    parameters.Add($"{filter.Gender}");
                }

            }
            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Phone] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Phone}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Degree))
            {
                if (filter.Degree != "0")
                {
                    sqlScript = $"""
                        {sqlScript}
                            AND [Degree] = @{parameters.Count}
                    """;
                    parameters.Add($"{filter.Degree}");
                }

            }
            if (!string.IsNullOrWhiteSpace(filter.Speciality))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Speciality] = @{parameters.Count}
                    """;
                parameters.Add($"{filter.Speciality}");
            }
            var isStartDateHasValue = filter.DateofBirthShowStart is not null;
            var isEndDateHasValue = filter.DateofBirthShowEnd != null;
            if (isStartDateHasValue && isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (Year(GETDATE()) - YEAR([DateofBirth]) between @{parameters.Count} and @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.DateofBirthShowStart);
                parameters.Add(filter.DateofBirthShowEnd);
            }
            else if (isStartDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (Year(GETDATE()) - YEAR([DateofBirth]) >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.DateofBirthShowStart);
            }
            else if (isEndDateHasValue)
            {


                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                (Year(GETDATE()) - YEAR([DateofBirth]) <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.DateofBirthShowEnd);
            }


        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.VolunteerRegistration>(sqlScript, [.. parameters]);
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
        var ret = db.Update<DataAccess.Models.AdminPages.VolunteerRegistration>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.VolunteerRegistration>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
