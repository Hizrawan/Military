// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;

/// <summary>
/// 
/// </summary>
[Table("EducationTrainings")]
public class PersonelMaintenance
{
    /// <summary>
    /// 
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Time { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? Notes { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? Quota { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? contact { get; set; }



    /// <summary>
    /// 
    /// </summary>
    public string? GuideUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Organizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CourseFee { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public bool IsPub { get; set; }

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
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DeleteBy { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? IsPubText { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [NotNull]
    public string? CourseTopic { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CourseTeacher { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool MealsProvided { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Coorganizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Participant { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CourseOutline { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CourseLocation { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SuitablePeople { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? TeacherIntroduction { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RegistrationInstructions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? DeleteDate { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public TimeSpan CourseTimeEnd { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TimeSpan CourseTimeStart { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? RegistrationEnd { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public DateTime? PublicationDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? RegistrationStart { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime CourseDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

}

