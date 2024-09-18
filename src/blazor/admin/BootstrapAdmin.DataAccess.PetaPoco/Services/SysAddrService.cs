// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class SysAddrService : ISysAddr
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>    
    public SysAddrService(IDBManager db)
        => DBManager = db;

    public List<SysAddr> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<SysAddr>();
    }

    public Dictionary<string, string> GetAllPRO()
    {
        var sysAddrs = GetAll();
        return sysAddrs.Select(d => new KeyValuePair<string, string>(d.PRO_ID, d.PRO_DESC)).Distinct().ToDictionary(i => i.Key, i => i.Value);
    }

    public Dictionary<string, string> GetCountrys(string proId)
    {
        var sysAddrs = GetAll();
        return sysAddrs.Where(s => s.PRO_ID == proId).Select(d => new KeyValuePair<string, string>(d.COUNTY_ID, d.COUNTY_DESC)).Distinct().ToDictionary(i => i.Key, i => i.Value);
    }
}
