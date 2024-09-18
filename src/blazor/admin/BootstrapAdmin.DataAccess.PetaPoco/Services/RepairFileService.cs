// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;

namespace BootstrapAdmin.DataAccess.PetaPoco.Services;

internal class RepairFileService : IRepairFile
{
    private IDBManager DBManager { get; }

    /// <summary>
    /// 建構子
    /// </summary>
    /// <param name="db"></param>
    public RepairFileService(IDBManager db)
        => DBManager = db;

    public IList<RepairFile> GetByDataId(int DataId, string fileType = "")
    {
        var others = "";
        if (fileType == "0")
        {
            others = "Bulletin";
        }
        if (fileType == "1")
        {
            others = "Events";
        }
        if (fileType == "2")
        {
            others = "EducationTrainings";
        }
        var othertype = 0;
        if (fileType == "0")
        {
            othertype = 1;
        }
        if (fileType == "1")
        {
            othertype = 2;
        }
        if (fileType == "2")
        {
            othertype = 3;
        }
        if (fileType == "DownloadArea")
        {
            othertype = 5;
        }
        if (fileType == "PhotoGallery")
        {
            othertype = 7;
        }
        if (fileType == "VN")
        {
            fileType = "VolunteerNeed";
        }
        if (fileType == "DownloadArea")
        {
            using var db = DBManager.Create();
            return db.Fetch<RepairFile>("WHERE [Id] = @0 AND ([FileType] = @1 OR [FileType] = @2 OR [FileType] = @3) AND [IsDelete] = 0", DataId, fileType.ToString(), othertype.ToString(), others);
        }
        else
        {
            using var db = DBManager.Create();
            return db.Fetch<RepairFile>("WHERE [DataId] = @0 AND ([FileType] = @1 OR [FileType] = @2 OR [FileType] = @3) AND [IsDelete] = 0", DataId, fileType.ToString(), othertype.ToString(), others);
        }

    }

    public IList<RepairFile> GetByDataIdAndFileName(int DataId, string fileName, string fileType = "")
    {
        using var db = DBManager.Create();
        return db.Fetch<RepairFile>("WHERE [DataId] = @0 AND [FileName]= @1 AND [FileType] = @2 AND [IsDelete] = 0", DataId, fileName, fileType);
    }

    public string? GetFileDescByFilePath(string preUrl)
    {
        using var db = DBManager.Create();
        return db.FirstOrDefault<string?>("SELECT [FileDesc] FROM [RepairFiles] WHERE [FilePath] + [FileName] = @0 AND [IsDelete] = 0", preUrl);
    }

}
