// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.DataAccess.Models;
using PetaPoco;
using System.Reflection;
using BootstrapAdmin.DataAccess.Models.AdminPages;

namespace BootstrapAdmin.DataAccess.PetaPoco;

class BootstrapAdminConventionMapper : ConventionMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pocoType"></param>
    /// <returns></returns>
    public override TableInfo GetTableInfo(Type pocoType)
    {
        var ti = base.GetTableInfo(pocoType);
        ti.AutoIncrement = true;

        // 支持 Oracle 数据库
        ti.SequenceName = $"SEQ_{ti.TableName.ToUpperInvariant()}_ID";

        ti.TableName = pocoType.Name switch
        {
            "Error" => "Exceptions",
            _ => $"{pocoType.Name}s"
        };
        return ti;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pocoProperty"></param>
    /// <returns></returns>
    public override ColumnInfo GetColumnInfo(PropertyInfo pocoProperty) => pocoProperty.DeclaringType?.Name switch
    {
        nameof(Models.User) => GetUserColumnInfo(pocoProperty),
        nameof(Models.Navigation) => GetNavigationColumnInfo(pocoProperty),
        nameof(Models.Group) => GetGroupColumnInfo(pocoProperty),
        nameof(Models.ApLog) => GetApLogColumnInfo(pocoProperty),
        nameof(Models.RepairLog) => GetRepairLogColumnInfo(pocoProperty),
        nameof(Models.SendEmail) => GetSendEmailColumnInfo(pocoProperty),
        nameof(Models.RegisterOnline) => GetRegisterOnlineColumnInfo(pocoProperty),
        nameof(EducationTraining) => GetEducationTrainingColumnInfo(pocoProperty),
        nameof(VolunteerRegistration) => GetVolunteerColumnInfo(pocoProperty),
        nameof(VolunteerNeed) => GetVolunteerNeedColumnInfo(pocoProperty),
        nameof(Bulletin) => GetBulletinColumnInfo(pocoProperty),
        nameof(PhotoGallery) => GetBulletinColumnInfo(pocoProperty),
        nameof(Event) => GetEventColumnInfo(pocoProperty),
        _ => base.GetColumnInfo(pocoProperty)

    };

    private ColumnInfo GetUserColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.User.NewPassword),
            nameof(Models.User.ConfirmPassword),
            nameof(Models.User.Period),
            nameof(Models.User.IsReset),
            nameof(Models.User.UserTypeValue),
            nameof(Models.User.VendorName)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }
    private ColumnInfo GetEducationTrainingColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
           nameof(EducationTraining.IsPubText),

        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }
    private ColumnInfo GetVolunteerColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
           nameof(VolunteerRegistration.GenderText),
           nameof(VolunteerRegistration.DegreeText),
           nameof(VolunteerRegistration.SpecialityText),
            nameof(VolunteerRegistration.DateofBirthShow),
             nameof(VolunteerRegistration.DateofBirthShowStart),
              nameof(VolunteerRegistration.DateofBirthShowEnd),
              nameof(VolunteerRegistration.HaveRecordText),


        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }
    private ColumnInfo GetVolunteerNeedColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
           nameof(VolunteerNeed.StatusText),
           nameof(VolunteerNeed.VerifyAnswer),
           nameof(VolunteerNeed.VerifyCode),


        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }
    private ColumnInfo GetEventColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
           nameof(Event.IsPubText)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }
    private ColumnInfo GetBulletinColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
           nameof(Bulletin.IsPubText)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }

    private ColumnInfo GetNavigationColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.Navigation.HasChildren)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }

    private ColumnInfo GetGroupColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.Group.GroupTypeName)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }


    private ColumnInfo GetApLogColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.ApLog.UpdByValue),
            nameof(Models.ApLog.ChgTypeValue)
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }


    private ColumnInfo GetRepairLogColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.RepairLog.OldRepairStatusValue),
            nameof(Models.RepairLog.NewRepairStatusValue),
            nameof(Models.RepairLog.CreateByValue),
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }

    private ColumnInfo GetSendEmailColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.SendEmail.EmailStatusValue),
            nameof(Models.SendEmail.EmailTypeValue),
        };
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }

    private ColumnInfo GetRegisterOnlineColumnInfo(PropertyInfo pocoProperty)
    {
        var ci = base.GetColumnInfo(pocoProperty);
        var resultColumns = new List<string>
        {
            nameof(Models.RegisterOnline.IsSent),
            nameof(Models.RegisterOnline.CourseTopic),
            nameof(Models.RegisterOnline.ToBeConfirmed),

            nameof(Models.RegisterOnline.quota),

            nameof(Models.RegisterOnline.Time),

            nameof(Models.RegisterOnline.CourseLocation),

            nameof(Models.RegisterOnline.Content),

            nameof(Models.RegisterOnline.CourseTeacher),
            nameof(Models.RegisterOnline.MealText),

            nameof(Models.RegisterOnline.IsVegetarianText),

            nameof(Models.RegisterOnline.StartDate),

            nameof(Models.RegisterOnline.EndDate),

            nameof(Models.RegisterOnline.CourseDate),
            nameof(Models.RegisterOnline.IsPub),
            nameof(Models.RegisterOnline.IsShow),

            nameof(Models.RegisterOnline.ShowFront),

            nameof(Models.RegisterOnline.MailTitle),

            nameof(Models.RegisterOnline.CategoryCode),

            nameof(Models.RegisterOnline.DataId),
            nameof(Models.RegisterOnline.EmailType),

            nameof(Models.RegisterOnline.EmailStatus),

            nameof(Models.RegisterOnline.Priority),
};
        ci.ResultColumn = resultColumns.Any(c => c == ci.ColumnName);
        return ci;
    }


    public override Func<object?, object?> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType) => targetProperty.PropertyType.IsEnum && sourceType == typeof(string)
        ? new NumberToEnumConverter(targetProperty.PropertyType).ConvertFromDb
        : base.GetFromDbConverter(targetProperty, sourceType);

    public override Func<object?, object?> GetToDbConverter(PropertyInfo targetProperty) => targetProperty.PropertyType.IsEnum
        ? new NumberToEnumConverter(targetProperty.PropertyType).ConvertToDb
        : base.GetToDbConverter(targetProperty);
}
