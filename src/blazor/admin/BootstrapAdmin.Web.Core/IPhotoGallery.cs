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
public interface IPhotoGallery
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFileNames(string Id);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool DeleteFileName(string Id, string? FileName);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<DataAccess.Models.AdminPages.PhotoGallery> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.PhotoGallery>> GetAllFrontAsync(PhotoGalleryFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.PhotoGallery>> GetFrontAsync(PhotoGalleryFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.PhotoGallery>> GetAllAsync(PhotoGalleryFilter? filter = null);


}
