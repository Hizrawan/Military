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
public interface ILeaveMessage
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<DataAccess.Models.AdminPages.LeaveMessage> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.LeaveMessage>> GetAllFrontAsync(LeaveMessageFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.LeaveMessage>> GetFrontAsync(LeaveMessageFilter? filter = null);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<DataAccess.Models.AdminPages.LeaveMessage>> GetAllAsync(LeaveMessageFilter? filter = null);


}
