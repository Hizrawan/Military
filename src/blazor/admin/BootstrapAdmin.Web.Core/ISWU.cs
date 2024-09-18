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
public interface ISWU
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<DataAccess.Models.AdminPages.SWU> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.SWU>> GetAllFrontAsync(SWUFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.SWU>> GetFrontAsync(SWUFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.SWU>> GetAllAsync(SWUFilter? filter = null);


}
