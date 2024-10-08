﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapBlazor.Components;
using BootstrapBlazor.DataAccess.PetaPoco;
using PetaPoco.Extensions;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

/// <summary>
/// PetaPoco ORM 的 IDataService 介面實現
/// </summary>
internal class DefaultDataService<TModel> : DataServiceBase<TModel> where TModel : class, new()
{
    private IDBManager DBManager { get; }

    private IUser UserService { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    /// <param name="userService"></param>
    public DefaultDataService(IDBManager db, IUser userService)
        => (DBManager, UserService) = (db, userService);

    /// <summary>
    /// 刪除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override Task<bool> DeleteAsync(IEnumerable<TModel> models)
    {
        // 通過模型獲取主鍵列資料
        // 支持批量刪除
        using var db = DBManager.Create();
        db.DeleteBatch(models);
        return Task.FromResult(true);
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    public override async Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
    {
        if (model is User user)
        {
            UserService.SaveUser(user.UserName, user.DisplayName, user.NewPassword, user.GroupsID, user.UserSID, user.UserEmail, user.UserTelPhone, user.UserCellPhone, user.IsEnable, user.UserType, user.VendorID);
        }
        else
        {
            using var db = DBManager.Create();
            if (changedType == ItemChangedType.Add)
            {
                await db.InsertAsync(model);
            }
            else
            {
                await db.UpdateAsync(model);
            }
        }
        return true;
    }

    /// <summary>
    /// 查詢方法
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public override async Task<QueryData<TModel>> QueryAsync(QueryPageOptions option)
    {
        var ret = new QueryData<TModel>()
        {
            IsSorted = option.SortOrder != SortOrder.Unset,
            IsFiltered = option.Filters.Any(),
            IsAdvanceSearch = option.AdvanceSearches.Any(),
            IsSearch = option.Searches.Any() || option.CustomerSearches.Any()
        };

        using var db = DBManager.Create();
        if (option.IsPage)
        {
            var items = await db.PageAsync<TModel>(option.PageIndex, option.PageItems, option.ToFilter(), option.SortName, option.SortOrder);

            ret.TotalCount = Convert.ToInt32(items.TotalItems);
            ret.Items = items.Items;
        }
        else
        {
            var items = await db.FetchAsync<TModel>(option.ToFilter(), option.SortName, option.SortOrder);
            ret.TotalCount = items.Count;
            ret.Items = items;
        }
        return ret;
    }
}
