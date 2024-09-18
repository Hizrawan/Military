// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.DataAccess.PetaPoco.Services;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Services;
using BootstrapAdmin.Web.Validators;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Roles
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    [Inject]
    [NotNull]
    private IGroup? GroupService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    [Inject]
    [NotNull]
    private IApp? AppService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private INavigation? NavigationService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private WebClientService? WebClientService { get; set; }

    [Inject]
    [NotNull]
    private ICommon<Role>? CommonService { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Roles>? Localizer { get; set; }

    private ITableSearchModel RolesSearchModel { get; } = new RolesSearchModel();

    private string? AddModalTitle;
    private string? EditModalTitle;

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
       
        AddModalTitle = Localizer["AddModalTitle"];
        EditModalTitle = Localizer["EditModalTitle"];
    }

        private Task<QueryData<Role>> OnQueryAsync(QueryPageOptions options)
    {
        var items = RoleService.GetAll();

        // 處理模糊查詢
        if (options.Searches.Any())
        {
            items = items.Where(options.Searches.GetFilterFunc<Role>(FilterLogic.Or)).ToList();
        }

        //  處理 Filter 高級搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<Role>()).ToList();
        }

        return Task.FromResult(new QueryData<Role>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = items
        });
    }

    private Task OnAfterModifyAsync()
    {
        CacheManager.Clear();
        return Task.FromResult(true);
    }

    private async Task<bool> OnDeleteAsync(IEnumerable<Role> roles)
    { 
        string? PGName = Localizer["DelLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = "D";
        string loginIP = (await WebClientService.GetClientInfo()).Ip!;

        // 這邊不做實際刪除
        // 改成調用公共方法
        var ret = await CommonService.DeleteAsync(roles, AppContext.UserName, chgType, PGName, PGCode, loginIP);

        return ret;
    }

    private async Task OnAssignmentUsers(Role role)
    {
        var users = UserService.GetAll().ToSelectedItemList();
        var values = UserService.GetUsersByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentUsersDialog"]}{role.RoleName}", users, values, () =>
        {
            var ret = UserService.SaveUsersByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentGroups(Role role)
    {
        var groups = GroupService.GetAll()
            .ToSelectedItemList();

        var values = GroupService.GetGroupsByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentGroupsDialog"]}{role.RoleName}", groups, values, () =>
        {
            var ret = GroupService.SaveGroupsByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentMenus(Role role)
    {
        var menus = NavigationService.GetAllMenus(AppContext.UserName).Where(menu => menu.IsEnable).ToList();
        var values = NavigationService.GetMenusByRoleId(role.Id);

        await DialogService.ShowNavigationDialog($"{Localizer["AssignmentMenusDialog"]}{role.RoleName}", menus, values, items =>
        {
            var ret = NavigationService.SaveMenusByRoleId(role.Id, items);
            return Task.FromResult(ret);
        });
    }

    private async Task OnAssignmentApps(Role role)
    {
        var apps = DictService.GetApps();
        var values = AppService.GetAppsByRoleId(role.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentAppsDialog"]}{role.RoleName}", apps.ToSelectedItemList(), values, () =>
        {
            var ret = AppService.SaveAppsByRoleId(role.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="role"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    private async Task<bool> OnSaveAsync(Role role, ItemChangedType changedType)
    {
        if (role == null) return false;

        string? PGName = changedType == ItemChangedType.Add ? Localizer["AddLogPGName"] : Localizer["EditLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = changedType == ItemChangedType.Add ? "I" : "U";
        string loginIP = (await WebClientService.GetClientInfo()).Ip!;

        var ret = await CommonService.SaveAsync(role, (BootstrapAdmin.DataAccess.Models.ItemChangedTypeAction)changedType, AppContext.UserName, chgType, PGName, PGCode, loginIP);

        return ret;
    }
}
