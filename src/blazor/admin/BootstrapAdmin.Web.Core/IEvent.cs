// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IEvent
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Event>> GetAllFrontAsync(EventFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Event>> GetFrontAsync(EventFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<Event>> GetAllAsync(EventFilter? filter = null);

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
    List<string>? GetFileNames(string Id);

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //Task<List<EventCarousel>> GetPaths();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Event> GetAll();

}
