// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;

internal class VolunteerNeedService : IVolunteerNeed
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public VolunteerNeedService(IDBManager db)
        => DBManager = db;

    public IEnumerable<DataAccess.Models.AdminPages.VolunteerNeed> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<DataAccess.Models.AdminPages.VolunteerNeed>().Where(b => !b.IsDelete);
    }
    public async Task<List<DataAccess.Models.AdminPages.VolunteerNeed>> GetAllFrontAsync(VolunteerNeedFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
            SELECT  
            		[id]
                  ,[UnitName]
                  ,[UnitAddress]
                  ,[ContactPerson]
                  ,[Phone]
                  ,[Mobile]
                  ,[Fax]
                  ,[Email]
                  ,[ServicePeriode]
                  ,[FirstServiceTime]
                  ,[SecondServiceTime]
                  ,[ServiceRemark]
                  ,[ServiceLocation]
                  ,[ServiceObject]
                  ,[ServiceContent]
                  ,[RequiredPeople]
                  ,FORMAT([StartDate], 'yyyy-MM-dd') AS [StartDate]
                  ,FORMAT([EndDate], 'yyyy-MM-dd') AS [EndDate]
                  --,CAST(CONVERT(date, [StartDate]) AS varchar) + ' ' + CAST([FirstServiceTime] AS varchar) AS [StartDate]
                  --,CAST(CONVERT(date, [EndDate]) AS varchar) + ' ' + CAST([SecondServiceTime] AS varchar) AS [EndDate]
                  ,[VolunteerCondition]
                  ,[Status]
                  ,[ReturnReason]
                  ,[CreateDate]
                  ,[IsEnable]
                  ,[IsDelete]
                  ,[CreateBy]
                  ,[ModifyBy]
                  ,[ModifyDate]
                  ,[DeleteBy]
                  ,[DeleteDate]
              FROM [VSPC].[dbo].[VolunteerNeed]

                WHERE [IsDelete] = 0
            and status =1
            and ((StartDate < GETDATE() and EndDate > GETDATE()) or (StartDate is null or EndDate is null))
                                   
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


        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;

        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.VolunteerNeed>(sqlScript, [.. parameters]);
        return contracts;
    }

    public async Task<List<DataAccess.Models.AdminPages.VolunteerNeed>> GetFrontAsync(VolunteerNeedFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
              SELECT  
            		[id]
                  ,[UnitName]
                  ,[UnitAddress]
                  ,[ContactPerson]
                  ,[Phone]
                  ,[Mobile]
                  ,[Fax]
                  ,[Email]
                  ,[ServicePeriode]
                  ,[FirstServiceTime]
                  ,[SecondServiceTime]
                  ,[ServiceRemark]
                  ,[ServiceLocation]
                  ,[ServiceObject]
                  ,[ServiceContent]
                  ,[RequiredPeople]
                  ,[StartDate]
                  ,[EndDate]
                  ,[VolunteerCondition]
                  ,[Status]
                  ,[ReturnReason]
                  ,[CreateDate]
                  ,[IsEnable]
                  ,[IsDelete]
                  ,[CreateBy]
                  ,[ModifyBy]
                  ,[ModifyDate]
                  ,[DeleteBy]
                  ,[DeleteDate]
              FROM [VSPC].[dbo].[VolunteerNeed]
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

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.VolunteerNeed>(sqlScript, [.. parameters]);
        return contracts;
    }
    public async Task<List<DataAccess.Models.AdminPages.VolunteerNeed>> GetAllAsync(VolunteerNeedFilter? filter = null)
    {
        using var db = DBManager.Create();
        var sqlScript = """
             SELECT  
            		[id]
                  ,[UnitName]
                  ,[UnitAddress]
                  ,[ContactPerson]
                  ,[Phone]
                  ,[Mobile]
                  ,[Fax]
                  ,[Email]
                  ,[ServicePeriode]
                  ,[FirstServiceTime]
                  ,[SecondServiceTime]
                  ,[ServiceRemark]
                  ,[ServiceLocation]
                  ,[ServiceObject]
                  ,[ServiceContent]
                  ,[RequiredPeople]
                  ,[StartDate]
                  ,[EndDate]
                  ,[VolunteerCondition]
                  ,[Status]
                  ,[ReturnReason]
                  ,[CreateDate]
                  ,[IsEnable]
                  ,[IsDelete]
                  ,[CreateBy]
                  ,[ModifyBy]
                  ,[ModifyDate]
                  ,[DeleteBy]
                  ,[DeleteDate]
              FROM [VSPC].[dbo].[VolunteerNeed]
                WHERE [IsDelete] = 0
            """;

        List<object> parameters = [];

        #region -- 進階搜尋條件 --

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.UnitName))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [UnitName] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.UnitName}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.UnitAddress))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [UnitAddress] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.UnitAddress}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.ContactPerson))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [ContactPerson] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.ContactPerson}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Phone))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Phone] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.Phone}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.ServiceContent))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [ServiceContent] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.ServiceContent}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.RequiredPeople))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [RequiredPeople] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.RequiredPeople}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.State))
            {
                sqlScript = $"""
                        {sqlScript}
                            AND [Status] LIKE @{parameters.Count}
                    """;
                parameters.Add($"%{filter.State}%");
            }

        }
        sqlScript = $"""
                {sqlScript}
                    ORDER BY [CreateDate] DESC
                """;
        #endregion

        var contracts = await db.FetchAsync<DataAccess.Models.AdminPages.VolunteerNeed>(sqlScript, [.. parameters]);
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
        var ret = db.Update<DataAccess.Models.AdminPages.VolunteerNeed>("SET [FileName]=CASE WHEN [FileName] IS NULL OR [FileName] ='' THEN '' ELSE [FileName] + ',' END +  @1 WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<DataAccess.Models.AdminPages.VolunteerNeed>("SET [FileName]=REPLACE(REPLACE([FileName], ',' + @1, ''), @1, '') WHERE [Id] = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

}
