// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IEducationTraining
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<EducationTraining> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<EducationTraining>> GetAllAsync(EducationTrainingFilter? filter = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<EducationTraining>> GetAllFrontAsync(EducationTrainingFilter? filter = null);


}
