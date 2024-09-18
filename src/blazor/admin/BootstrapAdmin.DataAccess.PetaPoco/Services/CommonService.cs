// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.Models.AdminPages;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;
using BootstrapAdmin.DataAccess.PetaPoco.Services.AdminPages;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Org.BouncyCastle.Asn1.Cmp;
using System.Reflection;


namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class CommonService<TModel> : ICommon<TModel> where TModel : class, new()
{
    private IDBManager DBManager { get; }
    [Inject]
    [NotNull]
    private IVolunteerNeed? volunteerNeedServices { get; set; }
    private IApLog AplogService { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    public CommonService(IDBManager db, IApLog aplogService)
        => (DBManager, AplogService) = (db, aplogService);
    public bool SetPropertyValue(TModel model, string propertyName, object value)
    {
        bool ret = false;
        if (model == null)
        {
            throw new ArgumentNullException();
        }

        Type modelType = typeof(TModel);
        PropertyInfo? propertyInfo = modelType.GetProperty(propertyName);

        if (propertyInfo != null)
        {
            propertyInfo.SetValue(model, value, null);

            ret = true;

        }
        return ret;
    }

    public object? GetPropertyValue(TModel model, string propertyName)
    {
        if (model == null)
        {
            throw new ArgumentNullException();
        }

        Type modelType = typeof(TModel);
        PropertyInfo? propertyInfo = modelType.GetProperty(propertyName);

        if (propertyInfo != null)
        {
            return propertyInfo.GetValue(model);
        }

        return null;
    }

    /// <summary>
    /// 刪除方法 (適用于刪除一筆資料, 然后做保存刪除操作記錄; 如刪除后還有其他邏輯, 需要另寫方法)
    /// </summary>
    /// <param name="models"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    public async Task<bool> DeleteAsync(IEnumerable<TModel> models, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        if (models == null)
        {
            return false;
        }

        bool ret;
        using var db = DBManager.Create();

        try
        {
            db.BeginTransaction();
            foreach (var model in models)
            {
                SetPropertyValue(model, "IsDelete", true);
                SetPropertyValue(model, "DeleteBy", userName);
                SetPropertyValue(model, "DeleteDate", DateTime.Now);
                if (model is Supervisor supervisors || model is SWU swus)
                {
                    var updRows = await db.UpdateAsync("TeamOrganization", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is VolunteerRegistration VR)
                {
                    if (VR.Gender == "男性")
                    {
                        VR.Gender = "0";
                    }
                    else if (VR.Gender == "女性")
                    {
                        VR.Gender += "1";
                    }
                    var updRows = await db.UpdateAsync("VolunteerRegistration", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is VolunteerNeed)
                {
                    var updRows = await db.UpdateAsync("VolunteerNeed", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is UnitMaintenance)
                {
                    var updRows = await db.UpdateAsync("Unit", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is PhotoGallery PG)
                {
                    PG.NewsType = "3";
                    var updRows = await db.UpdateAsync("Bulletins", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is DownloadArea DA)
                {
                    DA.FileType = "5";
                    var updRows = await db.UpdateAsync("FileUploaded", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is LeaveMessage LM)
                {

                    var updRows = await db.UpdateAsync("LeaveMessage", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is ProgramManagement PM)
                {

                    var updRows = await db.UpdateAsync("Program", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }

                else
                {
                    var updRows = 0;
                    if (typeof(TModel).GetProperty("IsDelete") != null)   //有IsDelete欄位, 做假刪除
                    {
                        SetPropertyValue(model, "IsDelete", true);
                        SetPropertyValue(model, "DeleteBy", userName);
                        SetPropertyValue(model, "DeleteDate", DateTime.Now);
                        updRows = db.Update(model);
                    }
                    else  // 實際刪除
                    {
                        updRows = db.Delete(model);
                    }

                    //apLog 2, 保存 log
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, null, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                //var Id = GetPropertyValue(model, "Id")?.ToString() ?? "";


            }
            db.CompleteTransaction();
            ret = true;
        }
        catch (Exception)
        {
            db.AbortTransaction();
            throw;
        }
        if (ret)
        {
            CacheManager.Clear();  // TODO, 是否都要清除
        }
        return ret;

        #region 原先更新刪除標記方法，暫存
        ///////////////  原先更新刪除標記方法，暫存
        //try
        //{
        //    db.BeginTransaction();
        //    foreach (var model in models)
        //    {
        //        //做刪除標記
        //        var Id = GetPropertyValue(model, "Id")?.ToString() ?? "";
        //        if (!string.IsNullOrEmpty(Id))
        //        {
        //            ret = db.Update<TModel>("Set IsDelete = @1, DeleteBy = @2, DeleteDate = Getdate() Where ID = @0", Id, 1, userName) == 1;

        //            //apLog 2, 保存 log
        //            if (ret)
        //            {
        //                await AplogService.SaveChangesAsync(model, null, chgType, PGName, PGCode, userName, LoginIP);
        //            }
        //        }
        //    }
        //    db.CompleteTransaction();
        //    ret = true;
        //}
        //catch (Exception)
        //{
        //    db.AbortTransaction();
        //    throw;
        //}
        //if (ret)
        //{
        //    CacheManager.Clear();  // TODO, 是否都要清除
        //}
        //return ret;
        #endregion
    }

    /// <summary>
    /// 資料保存方法(同時保存操作記錄, 適用于表格具有欄位: Id, CreateBy, CreateDate, ModifyBy, ModifyDate, DeleteBy, DeleteDate)
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <param name="userName"></param>
    /// <param name="chgType"></param>
    /// <param name="PGName"></param>
    /// <param name="PGCode"></param>
    /// <param name="LoginIP"></param>
    /// <returns></returns>
    public async Task<bool> SaveAsync(TModel model, ItemChangedTypeAction changedType, string userName, string? chgType = null, string? PGName = null, string? PGCode = null, string? LoginIP = null)
    {
        if (model == null)
        {
            return false;

        }
        using var db = DBManager.Create();

        try
        {
            if (changedType == ItemChangedTypeAction.Add)
            {
                //if (model is EducationTraining edutrain)
                //{
                //    string GetEmail = $"""
                //               SELECT [Email]
                //               FROM [NewsLetters] AS [News]
                //               JOIN [NewsletterMap] AS [Map] ON [Map].[NewsletterId] = [News].[Id]
                //               JOIN [SubscriptionClass] AS [Sub] ON [Sub].[Id] = [Map].[NewsLetterClass]
                //               WHERE [Sub].[Id] = @type
                //               """;

                //    var parameter = new { type = 2 };
                //    List<String> contracts = await db.FetchAsync<String>(GetEmail, parameter);
                //    string sqlScript = $""" SELECT ISNULL(max(id), 0) AS new_column_name from [EducationTrainings] """;

                //    var DataId = db.Fetch<int>(sqlScript);
                //    int dataid = DataId[0];

                //    var emailInserts = contracts.Select(contract => new
                //    {
                //        DataId = dataid + 1,
                //        EmailType = 3,
                //        EmailStatus = 0,
                //        Email = contract,
                //        Title = edutrain.CourseTopic,
                //        Content = "<B>新竹縣工商發展投資策進會-教育訓練課程通知</B> <br/><br/><table id=123 border='1'><tr><td bgcolor='yellow'>課程主題：</td><td>" + edutrain.CourseTopic + "</td></tr><tr><td bgcolor='yellow'>課程日期：</td><td>" + edutrain.CourseDate + "</td></tr><tr><td bgcolor='yellow'>報名期限：</td><td> " + edutrain.RegistrationStart + " - " + edutrain.RegistrationEnd + "</td></tr><tr><td bgcolor='yellow'>課程地點：</td><td>" + edutrain.CourseLocation + "</td></tr><tr><td bgcolor='yellow'>課程時間：</td><td>" + edutrain.CourseTimeEnd + " - " + edutrain.CourseTimeEnd + "</td></tr><tr><td bgcolor='yellow'>課程名額：</td><td>" + edutrain.Quota + "</td></tr><tr><td bgcolor='yellow'>課程費用：</td><td>" + edutrain.CourseFee + "</td></tr><tr><td bgcolor='yellow'>指導單位：</td><td>" + edutrain.GuideUnit + "</td></tr><tr><td bgcolor='yellow'>主辦單位：</td><td>" + edutrain.Organizer + "</td></tr><tr><td bgcolor='yellow'>提供餐點：</td><td>" + edutrain.MealsProvided + "</td></tr><tr><td bgcolor='yellow'>課程老師：</td><td>" + edutrain.CourseTeacher + "</td></tr><tr><td bgcolor='yellow'>課程老師介紹：</td><td>" + edutrain.TeacherIntroduction + "</td></tr></table><br/><a href='http://idipc.hsinchu.gov.tw'><b>歡迎報名參加(名額有限, 報完即止!)</b></a><br/>新竹縣工商發展投資策進會  聯絡電話: " + edutrain.contact + "",
                //        Priority = 3
                //    }).ToList();
                //    db.InsertBatch("SendEmails", emailInserts);
                //}

                //if (model is Event events)
                //{
                //    string GetEmail = $"""
                //               SELECT [Email]
                //               FROM [NewsLetters] AS [News]
                //               JOIN [NewsletterMap] AS [Map] ON [Map].[NewsletterId] = [News].[Id]
                //               JOIN [SubscriptionClass] AS [Sub] ON [Sub].[Id] = [Map].[NewsLetterClass]
                //               WHERE [Sub].[Id] = @type
                //               """;
                //    var parameter = new { type = 2 };
                //    List<String> contracts = await db.FetchAsync<String>(GetEmail, parameter);
                //    string sqlScript = $""" SELECT ISNULL(max(id), 0) AS new_column_name from [Events] """;

                //    var DataId = db.Fetch<int>(sqlScript);
                //    int dataid = DataId[0];

                //    var emailInserts = contracts.Select(contract => new
                //    {
                //        DataId = dataid + 1,
                //        EmailType = 3,
                //        EmailStatus = 0,
                //        Email = contract,
                //        events.Title,

                //        events.Content,
                //        Priority = 3
                //    }).ToList();

                //    // Perform the batch insert in one go
                //    db.InsertBatch("SendEmails", emailInserts);
                //    //db.InsertBatch("SendEmailLogs", emailLogInserts);
                //}
                if (userName == null)
                {
                    SetPropertyValue(model, "CreateBy", "FrontPages");
                }
                else
                {
                    SetPropertyValue(model, "CreateBy", userName);
                }

                SetPropertyValue(model, "CreateDate", DateTime.Now);

                if (model is Supervisor supervisors || model is SWU swus)
                {
                    var obj = await db.InsertAsync("TeamOrganization", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }

                else if (model is VolunteerNeed VN)
                {
                    var obj = await db.InsertAsync("VolunteerNeed", "Id", model);
                    if (obj != null)
                    {
                        await db.BeginTransactionAsync();
                        {
                            string sqlScript = $"""
                                               UPDATE [dbo].[RepairFiles]
                                              SET [DataID] = @Id

                                            WHERE DataID = 0
                                                
                                            """;
                            await db.ExecuteAsync(sqlScript, new { Id = VN.Id });
                        }
                        db.CompleteTransaction();
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is VolunteerRegistration)
                {
                    var obj = await db.InsertAsync("VolunteerRegistration", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is PhotoGallery PG)
                {
                    PG.NewsType = "3";
                    var obj = await db.InsertAsync("Bulletins", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is DownloadArea DA)
                {
                    DA.FileType = "5";
                    var obj = await db.InsertAsync("FileUploaded", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is LeaveMessage LM)
                {
                    LM.CreateBy = PGName;
                    LM.CreateDate = DateTime.Now;
                    LM.ReleaseDate = DateTime.Now;
                    LM.IsProcessed = false;
                    var obj = await db.InsertAsync("LeaveMessage", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is UnitMaintenance UM)
                {

                    var obj = await db.InsertAsync("Unit", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is Bulletin B)
                {
                    var obj = await db.InsertAsync("Bulletins", "Id", model);
                    if (obj != null)
                      
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }

                else if (model is ProgramManagement PM)
                {
                    if (PM.QualityCode is null)
                    {
                        PM.QualityCode = "0";
                    }

                    var obj = await db.InsertAsync("Program", "Id", model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else
                {
                    var obj = await db.InsertAsync(model);
                    if (obj != null)
                    {
                        await AplogService.SaveChangesAsync(null, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }


                //apLog , 保存 log


            }

            else
            {

                var Id = GetPropertyValue(model, "Id")?.ToString() ?? "";

                //當新增空報修單時，默認的IsDelete = true, 如果按下更新, 是要更新IsDelete = false
                //if (model is Repair) SetPropertyValue(model, "IsDelete", false);
                SetPropertyValue(model, "ModifyBy", userName);
                SetPropertyValue(model, "ModifyDate", DateTime.Now);

                if (model is Supervisor supervisors || model is SWU swus)
                {
                    var updRows = await db.UpdateAsync("TeamOrganization", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is VolunteerRegistration)
                {
                    var updRows = await db.UpdateAsync("VolunteerRegistration", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is VolunteerNeed)
                {
                    var updRows = await db.UpdateAsync("VolunteerNeed", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is PhotoGallery PG)
                {
                    PG.NewsType = "3";
                    var updRows = await db.UpdateAsync("Bulletins", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is DownloadArea DA)
                {
                    DA.FileType = "5";
                    var updRows = await db.UpdateAsync("FileUploaded", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is LeaveMessage LM)
                {

                    var updRows = await db.UpdateAsync("LeaveMessage", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else if (model is ProgramManagement PM)
                {
                    if (PM.QualityCode is null)
                    {
                        PM.QualityCode = "0";
                    }

                    var updRows = await db.UpdateAsync("Program", "Id", model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(model, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                else
                {
                    var orimodel = db.SingleOrDefault<TModel>("Where Id = @0", Id);
                    var updRows = await db.UpdateAsync(model);
                    if (updRows >= 1)
                    {
                        await AplogService.SaveChangesAsync(orimodel, model, chgType, PGName, PGCode, userName, LoginIP);
                    }
                }
                // 若模型是廠商，需要同步更新廠商類別
                //if (model is Vendor vendor)
                //{
                //    if (!string.IsNullOrWhiteSpace(vendor.VendorTypeMultiKey))
                //    {
                //        await db.BeginTransactionAsync();

                //        string sqlScript = $"""
                //            SELECT *
                //            FROM [VendorsTypeMapping] AS [vtm]
                //            WHERE [vtm].[VendorId] = @VendorId

                //        """;

                //        List<VendorsTypeMapping> vendorTypeMappings = db.Fetch<VendorsTypeMapping>(sqlScript, new { VendorId = vendor.Id });

                //        // 目前使用者修改後的資料
                //        string[] keys = vendor.VendorTypeMultiKey.Split(",");
                //        // 判斷資料表內是否存在沒有被使用者選擇的資料，若有則需要刪除
                //        foreach (string? id in vendorTypeMappings.Where(vtm => !keys.Contains(vtm.VendorTypeID))
                //            .Select(vtm => vtm.Id)
                //            .SkipWhile(id => id is null))
                //        {
                //            sqlScript = $"""
                //                DELETE FROM [VendorsTypeMapping] WHERE [Id] = @Id
                //            """;
                //            await db.ExecuteAsync(sqlScript, new { Id = id });
                //        }

                //        foreach (string key in keys)
                //        {
                //            // 檢查資料表內是否已存在指定的資料，若無則需要新增
                //            if (!vendorTypeMappings.Any(vtm => vtm.VendorTypeID == key))
                //            {
                //                sqlScript = $"""
                //                    INSERT INTO [VendorsTypeMapping]
                //                    (
                //                    [VendorID],
                //                    [VendorTypeID],
                //                    [CreateBy]
                //                    )
                //                    VALUES
                //                    (
                //                    @VendorID,
                //                    @VendorTypeID, @
                //                    CreateBy
                //                    )
                //                """;
                //                await db.ExecuteAsync(sqlScript, new { VendorID = vendor.Id, VendorTypeID = key, CreateBy = userName });
                //            }
                //        }
                //        db.CompleteTransaction();
                //    }
                //}
            }
            //apLog 1, 更新前 model





            return true;
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return false;
        }

    }

}
