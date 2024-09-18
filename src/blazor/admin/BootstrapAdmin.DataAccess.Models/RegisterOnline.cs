// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
[Table("RegisterOnlines")]
public class RegisterOnline
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? CourseTopic { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public int? ToBeConfirmed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public int? quota { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Time { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CourseLocation { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    public string? Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? CourseId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? CourseTeacher { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime Registrationdate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ApplicantName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? MealText { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? IsVegetarianText { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ContactPhone { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? CompanyAddress { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? MobilePhone { get; set; }

    /// <summary>
    ///
    /// </summary>
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool Meal { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsVegetarian { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsSent { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool Reply { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime? CourseDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsPub { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool IsDelete { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    public bool IsShow { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool ShowFront { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DeleteBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? MailTitle { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CategoryCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? DataId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? EmailType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? EmailStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Priority { get; set; }
}

