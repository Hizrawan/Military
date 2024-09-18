// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class VolunteerNeedFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public string? UnitName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? UnitAddress { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    public string? Phone { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    public string? ServiceContent { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    public string? RequiredPeople { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? FileDesc { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? VolunteerNeedEndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? VolunteerNeedStartDate { get; set; }
}
