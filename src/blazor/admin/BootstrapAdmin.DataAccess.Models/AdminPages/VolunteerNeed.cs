// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models.AdminPages;

/// <summary>
/// 
/// </summary>
[Table("VolunteerNeed")]
public class VolunteerNeed
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Required]
    public string? UnitName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string? UnitAddress { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? ContactPerson { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? Phone { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Mobile { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Fax { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "請輸入正確的email格式!")]
    public string? Email { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ServicePeriode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? FirstServiceTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? SecondServiceTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ServiceRemark { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? ServiceLocation { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? ServiceObject { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? ServiceContent { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? RequiredPeople { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <summary>
    ///

    /// </summary>
    [Required]
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    [Required]
    public string? VolunteerCondition { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Status { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? StatusText { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ReturnReason { get; set; }
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
    /// <summary>
    /// 
    /// </summary>
    public string? VerifyCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? VerifyAnswer { get; set; }

}

