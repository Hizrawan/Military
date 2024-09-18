// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IBulletin
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool DeleteFileName(string Id, string? FileName);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool SaveFileName(string Id, string? FileName);

    /// <summary>
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFileNamesHighlight(string Id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFileNamesNews(string Id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFileNames(string Id, string fileType, string other);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFilePath(string Id, string fileType, string other);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Bulletin> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetAllFrontAsync(BulletinFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<Bulletin> getimagedetail(BulletinFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetFrontAsync(BulletinFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetFrontEventAsync(BulletinFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetFrontEduAsync(BulletinFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetAllAsync(BulletinFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetAllEventAsync(BulletinFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetAllEduAsync(BulletinFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Bulletin>> GetAllPhotoAsync(BulletinFilter? filter = null);



}
