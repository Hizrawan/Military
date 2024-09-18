// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models.AdminPages;

/// <summary>
/// 
/// </summary>
[Table("TeamOrganization")]
public class Supervisor
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? OrganizationName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? Organizer { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? Phone { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? URL { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ServiceContent { get; set; }



    /// <summary>
    /// 
    /// </summary>
    public bool IsEnable { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsDelete { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? CreateBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? CreateDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ModifyBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? ModifyDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? DeleteBy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? DeleteDate { get; set; }

}

