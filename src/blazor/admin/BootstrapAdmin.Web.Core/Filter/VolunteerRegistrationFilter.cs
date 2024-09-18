// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone


namespace BootstrapAdmin.Web.Core;

/// <summary>
///
/// </summary>
public class VolunteerRegistrationFilter
{
    /// <summary>
    ///
    /// </summary>
    public string? Gender { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? Degree { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? Speciality { get; set; }

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
    public DateTime? VolunteerRegistrationEndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? VolunteerRegistrationStartDate { get; set; }
    public string? DateofBirthShowStart { get; set; }
    public string? DateofBirthShowEnd { get; set; }
}
