// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class SupervisorFilter
{
    /// <summary>
    ///
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Organizer { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? OrganizationName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? SupervisorEndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? SupervisorStartDate { get; set; }

    ///<summary>
    ///
    /// </summary>
    public string? Phone { get; set; }
    ///<summary>
    ///
    /// </summary>
    public string? URL { get; set; }

}
