// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface ISysAddr
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<SysAddr> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> GetAllPRO();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="proId"></param>
    /// <returns></returns>
    Dictionary<string, string> GetCountrys(string proId);

}
