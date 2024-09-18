// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 報修圖片實體類
/// </summary>
public interface IRepairFile
{

    /// <summary>
    /// 搜索報修單對應的照片
    /// </summary>
    /// <param name="DataId">Repairs.ID</param>
    /// <param name="fileType">R1(報修人上傳); 2(廠上偉)</param>
    /// <returns></returns>
    IList<RepairFile> GetByDataId(int DataId, string fileType = "");

    /// <summary>
    /// 搜索報修單對應的照片
    /// </summary>
    /// <param name="DataId">Repairs.ID</param>
    /// <param name="fileName">fileName</param>
    /// <param name="fileType">R1(報修人上傳); 2(廠上偉)</param>
    /// <returns></returns>
    IList<RepairFile> GetByDataIdAndFileName(int DataId, string fileName, string fileType = "");

    /// <summary>
    /// 取得圖片位址對應的圖片說明
    /// </summary>
    /// <param name="preUrl"></param>
    /// <returns></returns>
    string? GetFileDescByFilePath(string preUrl);
}
