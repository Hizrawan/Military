// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Pages.FrontPages.Details;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;

namespace BootstrapAdmin.Web.Pages.FrontPages;
/// <summary>
/// 
/// </summary>
public partial class FrontIndex
{
    [Inject]
    [NotNull]
    private IStringLocalizer<FrontIndex>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IBulletin? BulletinService { get; set; }


    private List<Bulletin>? Bulletins { get; set; }
    private Bulletin? imagedetail { get; set; }

    [Inject]
    [NotNull]
    private IEducationTraining? EducationTrainingService { get; set; }

    private List<Bulletin>? EducationTrainings { get; set; }
    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IEvent? EventService { get; set; }

    private List<Bulletin>? Events { get; set; }

    private string? SubscribeEmail { get; set; }

    private static List<string> Images =>
    [
        "./images/timthumb.jpeg",
        "./images/timthumb (1).jpeg",
        "./images/timthumb (2).jpeg"
    ];
    private Dictionary<(string tableName, string id), string> DataRowBackgroudColorKeyValues { get; set; } = [];
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }
    private Task ComponentOnClick(Bulletin Bulletins)
    {
        return DialogService.Show(new DialogOption()
        {
            Title = Bulletins.Title,
            BodyContext = Bulletins,
            BodyTemplate = builder =>
            {
                builder.OpenComponent<BulletinDetail>(0);
                builder.CloseComponent();
            }
        });
    }
    private Task ComponentEventOnClick(Bulletin Bulletins)
    {
        return DialogService.Show(new DialogOption()
        {
            Title = Bulletins.Title,
            BodyContext = Bulletins,
            BodyTemplate = builder =>
            {
                builder.OpenComponent<BulletinDetail>(0);
                builder.CloseComponent();
            }
        });
    }
    private Task ComponentEduTrainOnClick(Bulletin Bulletins)
    {
        return DialogService.Show(new DialogOption()
        {
            Title = Bulletins.Title,
            BodyContext = Bulletins,
            BodyTemplate = builder =>
            {
                builder.OpenComponent<BulletinDetail>(0);
                builder.CloseComponent();
            }
        });
    }
    private void OnDataRowMouseOver(MouseEventArgs args, string tableName, string? id)
    {
        if (!string.IsNullOrWhiteSpace(id))
        {
            DataRowBackgroudColorKeyValues[(tableName, id)] = "background-color: #D9D9D9;";
        }
    }

    private void OnDataRowMouseOut(MouseEventArgs args, string tableName, string? id)
    {
        if (!string.IsNullOrWhiteSpace(id))
        {
            DataRowBackgroudColorKeyValues[(tableName, id)] = string.Empty;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        imagedetail = await BulletinService.getimagedetail();
        imagedetail = imagedetail;
        Bulletins = await BulletinService.GetFrontAsync();
        Bulletins = Bulletins.OrderByDescending(d => d.CreateDate).Take(3).ToList();
        DataRowBackgroudColorKeyValueAddRange(Bulletins);

        Events = await BulletinService.GetFrontEventAsync();
        Events = Events.OrderByDescending(d => d.CreateDate).Take(3).ToList();
        DataRowBackgroudColorKeyValueAddRange(Events);

        EducationTrainings = await BulletinService.GetFrontEduAsync();
        EducationTrainings = EducationTrainings.OrderByDescending(d => d.CreateDate).Take(2).ToList();

        DataRowBackgroudColorKeyValueAddRange(EducationTrainings);

        //filePaths = await EventService.GetPaths();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public string RemoveHtmlTags(string? html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            return string.Empty;
        }
        var regex = MyRegex();
        var result = regex.Replace(html, " ");
        return result;
    }
    /// <summary>
    /// 
    /// </summary>

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(string email)
    {
        // Regular expression pattern for a valid email address
        string pattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$";

        // Case-insensitive matching
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

        return regex.IsMatch(email);
    }

    private void DataRowBackgroudColorKeyValueAddRange<T>(List<T> items)
    {
        foreach (T item in items)
        {
            string? id = item?.GetType()
                .GetProperty("Id")?
                .GetValue(item)?
                .ToString();

            if (item is not null && !string.IsNullOrWhiteSpace(id))
            {
                DataRowBackgroudColorKeyValues.Add((item.GetType().Name, id), string.Empty);
            }
        }
    }

    [GeneratedRegex("<.*?>")]
    private static partial Regex MyRegex();
}
