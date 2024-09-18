// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models.AdminPages;

/// <summary>
/// 
/// </summary>
[Table("VolunteerRegistration")]
public class VolunteerRegistration
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public string? Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public string? Gender { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    //[RegularExpression(@"^\d*$", ErrorMessage = "Invalid Number.")]
    public string? Phone { get; set; }
    /// <summary>
    /// 
    /// </summary>
    //[RegularExpression(@"^\d*$", ErrorMessage = "Invalid Number.")]
    public string? Mobile { get; set; }
    /// <summary>
    /// 
    /// </summary>
    //[RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "請輸入正確的email格式!")]
    public string? Email { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public string? Degree { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public string? Occupation { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? SchoolName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public string? Speciality { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? SpecialityText { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? DateofBirthShow { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? DateofBirthShowStart { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? DateofBirthShowEnd { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? GenderText { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? DegreeText { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public DateTime? DateofBirth { get; set; }
    /// <summary>
    /// 
    /// </summary>
    ///

    public string? ServiceType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? AbleFixed { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? NotAbleFixed { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Flexible { get; set; }

    ///// <summary>
    ///// 
    ///// </summary>
    //public DateTime? AvailableStartDate { get; set; }
    ///// <summary>
    ///// 
    ///// </summary>
    //public DateTime? AvailableEndDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool HaveRecord { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? HaveRecordText { get; set; }

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

