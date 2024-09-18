// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using PetaPoco;
using PetaPoco.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class ApLogService : IApLog
{
    private IDBManager DBManager { get; }

    private IStringLocalizer<ApLogService> Localizer { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="localizer"></param>
    public ApLogService(IDBManager db, IStringLocalizer<ApLogService> localizer)
        => (DBManager, Localizer) = (db, localizer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="IP"></param>
    /// <param name="OS"></param>
    /// <param name="browser"></param>
    /// <param name="address"></param>
    /// <param name="userAgent"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool Log(string userName, string? IP, string? OS, string? browser, string? address, string? userAgent, bool result)
    {
        //TODO
        var loginUser = new LoginLog()
        {
            UserName = userName,
            LoginTime = DateTime.Now,
            Ip = IP,
            City = address,
            OS = OS,
            Browser = browser,
            UserAgent = userAgent,
            Result = result ? Localizer["Success"] : Localizer["Failed"]
        };
        using var db = DBManager.Create();
        db.Insert(loginUser);

        //更新當前登入者的最近登入時間和IP
        var ret = db.Update<User>("Set LastLoginDate = @1, LastLoginIP = @2 Where UserName = @0", userName, DateTime.Now, IP) == 1;

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="searchText"></param>
    /// <param name="filter"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageItems"></param>
    /// <param name="sortList"></param>
    /// <returns></returns>
    public (IEnumerable<ApLog> Items, int ItemsCount) GetAll(string? searchText, ApLogFilter filter, int pageIndex, int pageItems, List<string> sortList)
    {
        var sql = new Sql();

        if (!string.IsNullOrEmpty(filter.ChgIP))
        {
            sql.Where("ChgIp Like @0 ", $"%{filter.ChgIP}%");
        }

        if (filter.Start.HasValue)
        {
            sql.Where("ChgTime >= @0 ", filter.Start);
        }
        if (filter.Start.HasValue)
        {
            sql.Where("ChgTime <= @0 ", filter.End);
        }
        //sql.Where("ChgTime >= @0 and ChgTime <= @1", filter.Start, filter.End);

        if (sortList.Any())
        {
            sql.OrderBy(string.Join(", ", sortList));
        }
        else
        {
            sql.OrderBy("ChgTime desc");
        }

        using var db = DBManager.Create();
        string sqlScript = $"""
                SELECT [Log].*, ISNULL([User].[DisplayName], '') AS [UpdByValue], ISNULL([Dic].[Name], '') AS [ChgTypeValue]
                FROM [ApLogs] AS [Log]
                LEFT JOIN [Users] AS [User] ON [Log].[UpdBy] = Convert(Varchar,[User].[Id])
                LEFT JOIN (Select * From Dicts Where Category = 'ChgType') AS [Dic] ON [Log].[ChgType] = [Dic].[Code] where 1=@0 Order By [Log].[Id] Desc
            """;

        var data = db.Page<ApLog>(pageIndex, pageItems, sqlScript, "1");  // TODO, 查詢條件要再加上
        return (data.Items, Convert.ToInt32(data.TotalItems));
    }

    /// <summary>
    /// 寫操作記錄
    /// </summary>
    /// <param name="oriObject">當新增時，傳入null</param>
    /// <param name="curObject">當刪除時，傳入null</param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginUserID"></param>
    /// <param name="LoginIP"></param>
    /// <param name="columns">只檢索當前的欄位是否有更新</param>
    /// <returns></returns>
    public async Task SaveChangesAsync(object? oriObject, object? curObject, string? chgType, string? PGName, string? PGCode, string? LoginUserID, string? LoginIP, IEnumerable<string>? columns = null)
    {
        if (oriObject == null && curObject == null)
            return;

        var IsSaveLog = false;  // 是否有異動到欄位
        var pocoData = oriObject == null ? PocoData.ForType(curObject.GetType(), new ConventionMapper()) : PocoData.ForType(oriObject.GetType(), new ConventionMapper());
        var tableName = pocoData.TableInfo.TableName;
        var primaryKeyName = pocoData.TableInfo.PrimaryKey;
        var LogValue = new List<ApLog>(); //資料異動紀錄檔
        var entityObject = pocoData.Columns;
        List<LogChgModel> changeModels = new List<LogChgModel>();
        List<string> lstFields = new List<string>();
        List<string> lstBefore = new List<string>();
        List<string> lstAfter = new List<string>();
        string PKValue = "";

        if (!string.IsNullOrWhiteSpace(primaryKeyName))
        {
            PKValue = (string)pocoData.Columns[primaryKeyName].GetValue(oriObject == null ? curObject : oriObject);
        }

        //檢索全部字段是否有更新
        if (columns == null)
        {
            foreach (KeyValuePair<string, PocoColumn> logfield in entityObject)
            {
                if (!logfield.Value.ResultColumn)
                {
                    var originalValue = oriObject == null ? "" : logfield.Value.GetValue(oriObject)?.ToString() ?? "";
                    var currentValue = curObject == null ? "" : logfield.Value.GetValue(curObject)?.ToString() ?? "";
                    var columnName = logfield.Key;

                    //修改模式-資料異動
                    ////if (entry.State == EntityState.Modified)
                    ////{
                    if (originalValue.ToString() == currentValue.ToString())
                    {
                        continue;
                    }

                    changeModels.Add(new LogChgModel
                    {
                        ColumnName = $"{columnName}",
                        BEFValue = $"{originalValue}",
                        AFTValue = $"{currentValue}",
                    });

                    lstFields.Add(columnName);
                    lstBefore.Add(originalValue!.ToString());
                    lstAfter.Add(currentValue!.ToString());
                    ////}
                    ////else if (entry.State == EntityState.Added)
                    ////{
                    ////    //新增模式-資料異動
                    ////    if (currentValue == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    changeModels.Add(new LogChgModel
                    ////    {
                    ////        ColumnName = $"{logfield.Name}",
                    ////        BEFValue = $"{originalValue}",
                    ////        AFTValue = $"{currentValue}",
                    ////    });

                    ////    lstFields.Add(logfield.Name);
                    ////    lstBefore.Add("");
                    ////    lstAfter.Add(currentValue.ToString());

                    ////}
                    ////else if (entry.State == EntityState.Deleted)
                    ////{
                    ////    //刪除模式-資料異動
                    ////    if (originalValue == null)
                    ////    {
                    ////        continue;
                    ////    }

                    ////    changeModels.Add(new LogChgModel
                    ////    {
                    ////        ColumnName = $"{logfield.Name}",
                    ////        BEFValue = $"{originalValue}",
                    ////        AFTValue = $"{currentValue}",
                    ////    });

                    ////    lstFields.Add(logfield.Name);
                    ////    lstBefore.Add(originalValue.ToString());
                    ////    lstAfter.Add("");
                    ////}
                    IsSaveLog = true;
                }
            }
        }
        else  //檢索部分字段是否有更新
        {
            foreach (string column2 in columns)
            {
                PocoColumn pocoColumn = pocoData.Columns[column2];

                var originalValue = oriObject == null ? "" : pocoColumn.GetValue(oriObject)?.ToString() ?? "";
                var currentValue = curObject == null ? "" : pocoColumn.GetValue(curObject)?.ToString() ?? "";
                var columnName = pocoColumn.PropertyInfo.Name;

                //資料異動
                if (originalValue.ToString() == currentValue.ToString())
                {
                    continue;
                }

                changeModels.Add(new LogChgModel
                {
                    ColumnName = $"{columnName}",
                    BEFValue = $"{originalValue}",
                    AFTValue = $"{currentValue}",
                });

                lstFields.Add(columnName);
                lstBefore.Add(originalValue!.ToString());
                lstAfter.Add(currentValue!.ToString());

                IsSaveLog = true;
            }
        }

        if (IsSaveLog)
        {
            //一筆資料列寫一筆Log
            var logModel = new ApLog
            {
                TableName = tableName,
                PKFIELD = primaryKeyName,
                PKValue = PKValue,
                ChgTime = DateTime.Now,
                PGName = PGName,
                PGCode = PGCode,
                UpdBy = LoginUserID,
                ChgType = chgType,  //待定, (entry.State == EntityState.Added) ? "I" : (entry.State == EntityState.Modified) ? "U" : (entry.State == EntityState.Deleted) ? "D" : "",
                ChgFIELD = string.Join("§", lstFields),
                BEFValue = string.Join("§", lstBefore),
                AFTValue = string.Join("§", lstAfter),
                ChgValue = JsonConvert.SerializeObject(changeModels),
                ChgIP = LoginIP
            };

            using var db = DBManager.Create();
            await db.InsertAsync(logModel);
        }
    }
}
