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

using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IUnitMaintenance
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<UnitMaintenance> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<UnitMaintenance>> GetAllFrontAsync(UnitMaintenanceFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<UnitMaintenance>> GetFrontAsync(UnitMaintenanceFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<UnitMaintenance>> GetAllAsync(UnitMaintenanceFilter? filter = null);


}
