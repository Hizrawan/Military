// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class AddressService : IAddress
{
    // private const string AddressServiceGetAllCacheKey = "AddressService-GetAll";

    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public AddressService(IDBManager db)
        => DBManager = db;

    public async Task<List<Address>> GetAllAsync() /* => CacheManager.GetOrAdd(AddressServiceGetAllCacheKey, entry => */
    {
        using var db = DBManager.Create();
        string sqlScript = $"""
                SELECT [adr].*
                FROM [Addresses] AS [adr]
            """;
        return await db.FetchAsync<Address>(sqlScript);
    }/*);*/
}
