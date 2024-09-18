// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class RegisterOnlineService : IRegisterOnline
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public RegisterOnlineService(IDBManager db)
        => DBManager = db;

    public IEnumerable<RegisterOnline> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<RegisterOnline>().Where(b => !b.IsDelete);
    }
    public async Task<List<RegisterOnline>> GetAllAsync(RegisterOnlineFilter? filter = null)
    {
        using var db = DBManager.Create();
        string sqlScript = """

                SELECT  DISTINCT
                        [R].[CourseId],
                        [E].[CourseTopic],
                        [CourseDate],
                        [E].[quota],
                        (
                            SELECT COUNT(*)
                            FROM   [RegisterOnlines] AS [counting]
                            WHERE  [counting].[CourseId] = [E].[Id]
                                AND [counting].[IsDelete] = 0
                        ) AS [ToBeConfirmed],
                        [E].[CourseTeacher],
                        [E].[CourseLocation],
                        (
                            SELECT CAST([E].[CourseTimeStart] AS VARCHAR(5)))+' - '+(SELECT CAST([E].[CourseTimeEnd] AS VARCHAR(5))
                        )AS [time]

                FROM  [RegisterOnlines] AS [R]
                    JOIN [EducationTrainings] AS [E]
                        ON [R].[CourseId] = [E].[Id]

                WHERE  [R].[IsDelete] = 0
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
                           AND [EndDate] > GETDATE()
                    """;
                }
                if (filter.State == "3")
                {
                    sqlScript = $"""
                        {sqlScript}
                           AND [isPub] = 1
                           AND [EndDate] < GETDATE()
                    """;
                }
                parameters.Add($"%{filter.State}%");
            }

            bool isStartDateHasValue = filter.RegisterOnlineStartDate.GetValueOrDefault() != default;
            bool isEndDateHasValue = filter.RegisterOnlineEndDate.GetValueOrDefault() != default;
            if (isStartDateHasValue && isEndDateHasValue)
            {
                
                
                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                ( [CourseDate]
                                    BETWEEN @{parameters.Count}
                                    AND @{parameters.Count + 1})
                            )
                    """;
                parameters.Add(filter.RegisterOnlineStartDate.GetValueOrDefault());
                parameters.Add(filter.RegisterOnlineEndDate.GetValueOrDefault());
            }
            else if (isStartDateHasValue)
            {
                
                
                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                ([CourseDate] >= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.RegisterOnlineStartDate.GetValueOrDefault());
            }
            else if (isEndDateHasValue)
            {
                // 無限期 ~ ContractEndDate
                // 資料的起始日和終止日需要小於 ContractEndDate
                sqlScript = $"""
                        {sqlScript}
                            AND
                            (
                                ([CourseDate] <= @{parameters.Count})
                            )
                    """;
                parameters.Add(filter.RegisterOnlineEndDate.GetValueOrDefault());
            }
            sqlScript = $"""
                        {sqlScript}
                            ORDER BY [CourseDate] DESC;
                    """;
        }
        #endregion

        List<RegisterOnline> contracts = await db.FetchAsync<RegisterOnline>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<RegisterOnline>> GetDetailEmail(string Id)
    {
        using var db = DBManager.Create();
        string sqlScript = """

                SELECT  [R].[Id],
                        [R].[ApplicantName],
                        [R].[Email],
                        [R].[CourseId],
                        [R].[Registrationdate],
                        [E].[CourseTopic],
                        [CourseDate],
                        [E].[quota],
                        (
                            SELECT COUNT(*)
                            FROM   [RegisterOnlines] AS [counting]
                            WHERE  [counting].[CourseId] = [E].[Id]
                        ) AS [ToBeConfirmed],
                        [E].[CourseTeacher]

                FROM [RegisterOnlines] AS [R]
                    JOIN EducationTrainings AS [E]
                         ON [R].[CourseId]	= [E].[Id] 
                WHERE [R].[IsDelete] = 0 
              
            """;

        List<object> parameters = [];
        if (Id is not null)
        {
            // 無限期 ~ ContractEndDate
            // 資料的起始日和終止日需要小於 ContractEndDate
            sqlScript = $"""
                        {sqlScript}
                            AND [CourseId] =@{parameters.Count}
                    """;
            parameters.Add(Id);
        }
        List<RegisterOnline> contracts = await db.FetchAsync<RegisterOnline>(sqlScript, [.. parameters]);
        return contracts;
    }

    public Dictionary<string, string> GetCategories()
    {
        var cat = category();
        return cat.Select(d => new KeyValuePair<string, string>(d.Value, d.Text)).Distinct().ToDictionary(i => i.Key, i => i.Value);
    }
    public List<SelectedItem> category()
    {
        using var db = DBManager.Create();
        var sql = "SELECT [Name] AS [Text], [Code] AS [Value] FROM [Dicts] WHERE [Category] = 'EmailType' AND [Code] LIKE 'ET%'";
        var categories = db.Fetch<SelectedItem>(sql);
        return categories;
    }
    public List<RegisterOnline> GetDetail(string category)
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT  DISTINCT
               	        [R].[Id],
               	        [R].[CourseId],
               	        [R].[Registrationdate],
               	        [R].[ApplicantName],
               	        [R].[CompanyName],
               	        [R].[JobTitle],
               	        [R].[ContactName],
               	        [R].[ContactPhone],
               	        [R].[CompanyAddress],
               	        [R].[Extension],
               	        [R].[Fax],
               	        [R].[MobilePhone],
               	        [R].[Email],
               	        [R].[Meal],
               	        [R].[IsVegetarian],
               	        [R].[IsSent],
               	        [R].[Reply],
               	        [R].[IsDelete],
               	        [R].[CreateBy],
               	        [R].[CreateDate],
               	        [R].[ModifyBy],
               	        [R].[ModifyDate],
               	        [R].[DeleteBy],
               	        [R].[DeleteDate],
               	        [E].[CourseTopic],
               	        [E].[CourseDate],
               	        [E].[quota],
                        [E].[CourseLocation],
                        (SELECT CAST([E].[CourseTimeStart] AS VARCHAR))+' - '+(SELECT CAST([E].[CourseTimeEnd] AS VARCHAR)) AS [time],
               	        (SELECT COUNT(*) FROM [RegisterOnlines] AS [counting] WHERE [counting].[CourseId] = @Category) AS [ToBeConfirmed],
               	        [E].[CourseTeacher]

                FROM [RegisterOnlines] AS [R]
               	    JOIN [EducationTrainings] AS [E] 
               	        ON [R].[CourseId] = [E].[Id]
            
                WHERE [R].[IsDelete] = 0
                    AND [E].[Id] = @Category
                    ORDER BY [CreateDate] DESC;
            """;
        return db.Fetch<RegisterOnline>(sqlScript, new { Category = category });
    }

    public List<RegisterOnline> GetDetailPrint(string category)
    {
        using var db = DBManager.Create();
        string sqlScript = """
                SELECT  DISTINCT
               	        [R].[Id],
               	        [R].[CourseId],
               	        [R].[Registrationdate],
               	        [R].[ApplicantName],
               	        [R].[CompanyName],
               	        [R].[JobTitle],
               	        [R].[ContactName],
               	        [R].[ContactPhone],
               	        [R].[CompanyAddress],
               	        [R].[Extension],
               	        [R].[Fax],
               	        [R].[MobilePhone],
               	        [R].[Email],
               	        [R].[Meal],
               	        [R].[IsVegetarian],
               	        [R].[IsSent],
               	        [R].[Reply],
               	        [R].[IsDelete],
               	        [R].[CreateBy],
               	        [R].[CreateDate],
               	        [R].[ModifyBy],
               	        [R].[ModifyDate],
               	        [R].[DeleteBy],
               	        [R].[DeleteDate],
               	        [E].[CourseTopic],
               	        [E].[CourseDate],
               	        [E].[quota],
                        [E].[CourseLocation],
                        (SELECT CAST([E].[CourseTimeStart] AS VARCHAR))+' - '+(SELECT CAST([E].[CourseTimeEnd] AS VARCHAR)) AS [time],
               	        (SELECT COUNT(*) FROM [RegisterOnlines] AS [counting] WHERE [counting].[CourseId] = @Category) AS [ToBeConfirmed],
               	        [E].[CourseTeacher]

                FROM [RegisterOnlines] AS [R]
                    JOIN [EducationTrainings] AS [E] 
                        ON [R].[CourseId] = [E].[Id]

                WHERE [R].[IsDelete] = 0
                     AND [E].[Id] = @Category

                     ORDER BY [CreateDate] DESC;
            """;
        return db.Fetch<RegisterOnline>(sqlScript, new { Category = category });
    }

    public async Task<bool> SaveAsync(List<RegisterOnline> registeronline, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        try
        {
            foreach (var item in registeronline)
            {
                if (item.CourseId != null)
                {
                    using var db = DBManager.Create();
                    var DataId = int.Parse(item.CourseId);
                    string sqlScript2 = $"""
                        INSERT INTO [SendEmails]
                            (
                                [DataId],
                                [EmailType],
                                [EmailStatus],
                                [Email],
                                [Title],
                                [Content],
                                [Priority],
                                [CreateDate]
                            )
                        VALUES
                            (
                                @DataId,
                                @EmailType,
                                @EmailStatus,
                                @Email,
                                @Title,
                                @Coontent,
                                @Priority,
                                @CreateDate
                            )

                        INSERT INTO [SendEmailLogs]
                              (
                                  [id],
                                  [DataId],
                                  [EmailType],
                                  [EmailStatus],
                                  [Email],
                                  [Title],
                                  [Content],
                                  [Priority]
                              )
                        VALUES
                              (
                                    1,
                                    @DataId,
                                    @EmailType,
                                    @EmailStatus,
                                    @Email,
                                    @Title,
                                    @Coontent,
                                    @Priority
                              )
                    """;
                    var test = await db.ExecuteAsync(sqlScript2, new
                    {
                        DataId = int.Parse(item.CourseId),
                        EmailType = item.CategoryCode,
                        EmailStatus = 3,
                        item.Email,
                        Title = item.MailTitle,
                        Coontent = item.Content,
                        Priority = 3,
                        item.CreateDate
                    });
                }
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return false;
        }
        return true;
    }
    public bool IsExist(RegisterFront registerFront)
    {
        try
        {
            using var db = DBManager.Create();

            string sqlScript2 = $"""
                       
                          SELECT CASE 
                            WHEN EXISTS (SELECT 1 FROM [RegisterOnlines] WHERE [Email] = @Email AND [CourseId]=@CourseId) 
                            THEN 1 
                            ELSE 0 
                        END AS [exist];
                    """;
            var exists = db.ExecuteScalarAsync<bool>(sqlScript2, new
            {
                registerFront.Email,
                CourseId = registerFront.Id,
            }).Result;
            return exists;
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return false;
        }
    }
    public async Task<bool> SaveFrontAsync(RegisterFront registerFront, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        try
        {
            using var db = DBManager.Create();
            if (registerFront.Id != null)
            {
                var DataId = int.Parse(registerFront.Id);
                string sqlScript2 = $"""
                        INSERT INTO [RegisterOnlines]
                              ([CourseId]
                              ,[Registrationdate]
                              ,[ApplicantName]
                              ,[CompanyName]
                              ,[JobTitle]
                              ,[ContactName]
                              ,[ContactPhone]
                              ,[CompanyAddress]
                              ,[Extension]
                              ,[Fax]
                              ,[MobilePhone]
                              ,[Email]
                              ,[Meal]
                              ,[IsVegetarian]
                              ,[IsSent]
                              ,[Reply]
                              ,[IsDelete])
                        VALUES
                              (
                                @DataId
                              ,GetDate()
                              ,@ApplicantName
                              ,@CompanyName
                              ,@JobTitle
                              ,@ContactName
                              ,@ContactPhone
                              ,@CompanyAddress
                              ,@Extension
                              ,@Fax
                              ,@MobilePhone
                              ,@Email
                              ,@Meal
                              ,@IsVegetarian
                              ,0
                              ,0
                              ,0
                              )


                        INSERT INTO [SendEmails]
                            (
                                [DataId],
                                [EmailType],
                                [EmailStatus],
                                [Email],
                                [Title],
                                [Content],
                                [Priority],
                                [CreateDate]
                            )
                        VALUES
                            (
                                @DataId,
                                @EmailType,
                                @EmailStatus,
                                @Email,
                                @Title,
                                @Coontent,
                                @Priority,
                                @CreateDate
                            )

                       
                    """;

                var test = await db.ExecuteAsync(sqlScript2, new
                {
                    registerFront.ApplicantName,
                    registerFront.CompanyName,
                    registerFront.JobTitle,
                    registerFront.ContactName,
                    registerFront.ContactPhone,
                    registerFront.CompanyAddress,
                    registerFront.Extension,
                    registerFront.Fax,
                    registerFront.MobilePhone,
                    DataId = int.Parse(registerFront.Id),
                    EmailType = 3,
                    EmailStatus = 3,
                    registerFront.Email,
                    Title = "Registered",
                    Coontent = "You are registered to this event",
                    Priority = 3,
                    registerFront.CreateDate,
                    registerFront.Meal,
                    registerFront.IsVegetarian
                });
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return false;
        }
        return true;
    }
}
