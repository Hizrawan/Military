// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class FormService : IForm
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>    
    public FormService(IDBManager db)
        => DBManager = db;

    public IEnumerable<Form> GetAll()
    {
        using var db = DBManager.Create();
        return db.Fetch<Form>().Where(b => !b.IsDelete);
    }

    public List<string>? GetFileNames(string Id)
    {
        using var db = DBManager.Create();
        var fileNames = db.Fetch<string>("select FileName from Forms where Id = @0", Id);

        if (fileNames is null)
            return fileNames;

        return fileNames[0]?.Split(",").ToList();
    }

    public Form GetFormAsync(string Id)
    {
        using var db = DBManager.Create();
        return db.FirstOrDefault<Form>("select * from Forms where Id = @0", Id);
    }
    
    public bool SaveFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<Form>("set FileName=case when FileName is null or FileName ='' then '' else FileName + ',' end +  @1 where Id = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }

    public bool DeleteFileName(string Id, string? FileName)
    {
        using var db = DBManager.Create();
        var ret = db.Update<Form>("set FileName=Replace(Replace(FileName, ',' + @1, ''), @1, '') where Id = @0", Id, FileName) == 1;
        if (ret)
        {
            CacheManager.Clear();
        }
        return ret;
    }
}
