// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Components;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Localization;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.DataAccess.Models.AdminPages.News;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
[AllowAnonymous]
public partial class Index
{
    [Inject]
    [NotNull]
    private IStringLocalizer<Index>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? Context { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictsService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UsersService { get; set; }

    [NotNull]
    private string? Url { get; set; }

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private IBulletin? BulletinsService { get; set; }

    [Inject]
    [NotNull]
    private IForm? FormsService { get; set; }

    [NotNull]
    private List<Bulletin>? bulletins { get; set; }

    [NotNull]
    private List<Form>? forms { get; set; }

    [NotNull]
    private List<string>? messages { get; set; }

    private static string? GetItemDisplayText(Bulletin bulletin) => bulletin.Title;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        //比較日期只取年月日比較
        var nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
        bulletins = BulletinsService.GetAll().Where(b => !b.IsDelete && b.IsPub && (!b.StartDate.HasValue || b.StartDate <= nowDate) && (!b.EndDate.HasValue || b.EndDate >= nowDate)).OrderByDescending(b => b.CreateDate).ToList();

        //TODO 待辦事項
        messages = new List<string> { "您有 6 張報修單需要派工！" };

        //表單下載
        forms = FormsService.GetAll().Where(b => !b.IsDelete && b.IsEnable && (!b.StartDate.HasValue || b.StartDate <= nowDate) && (!b.EndDate.HasValue || b.EndDate >= nowDate)).OrderByDescending(b => b.CreateDate).ToList();

        //每次都重新加載
        //StateHasChanged();
    }

    private RenderFragment<string> BodyTemplate = (content) => (RenderTreeBuilder builer) =>
    {
        builer.OpenElement(0, "div");
        builer.AddContent(0, (MarkupString)content);
        builer.CloseElement();
    };

    private async Task OnBulletinsListViewItemClick(Bulletin bulletin)
    {
        //Logger.Log($"ListViewItem: {item.Description} clicked");
        var op = new DialogOption()
        {
            //彈窗用法, 原HeaderTemplate默認顯示大一號字，不需要自己加大字體
            //Title = bulletin.Title,
            //BodyTemplate = BodyTemplate(bulletin.Content),
            //FooterTemplate = BootstrapDynamicComponent.CreateComponent<BulletinDownLoadFileList>(
            //    new Dictionary<string, object?>
            //    {
            //        [nameof(BulletinDownLoadFileList.Id)] = bulletin.Id!,
            //        [nameof(BulletinDownLoadFileList.TypeName)] = "Bulletin",
            //        [nameof(BulletinDownLoadFileList.UploadType)] = "2"
            //    }
            //).Render()

            //自己彈組件, 要對如標題字體大小做定義
            //Component = BootstrapDynamicComponent.CreateComponent<BulletinDetail>(
            //    new Dictionary<string, object?>
            //    {
            //        [nameof(BulletinDetail.Value)] = bulletin,
            //    }
            //),

        };
        await DialogService.Show(op);
    }

    private async Task OnFormsListViewItemClick(Form form)
    {
        var op = new DialogOption()
        {
            //自己彈組件, 要對如標題字體大小做定義
            Component = BootstrapDynamicComponent.CreateComponent<FormDetail>(
                new Dictionary<string, object?>
                {
                    [nameof(FormDetail.Value)] = form,
                }
            ),

        };
        await DialogService.Show(op);
    }
}
