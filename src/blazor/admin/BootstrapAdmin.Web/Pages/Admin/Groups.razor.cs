// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Caching;
using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using BootstrapAdmin.Web.Models;
using BootstrapAdmin.Web.Services;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Pages.Admin;

/// <summary>
/// 
/// </summary>
public partial class Groups
{
    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Groups>? Localizer { get; set; }

    #region -- Service --

    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private WebClientService? WebClientService { get; set; }
    
    [Inject]
    [NotNull]
    private ICommon<Group>? CommonService { get; set; }

    [Inject]
    [NotNull]
    private IAddress? AddressService { get; set; }

    [Inject]
    [NotNull]
    private IGroup? GroupService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    #endregion

    #region -- Model 和 SelectedItem --

    private ITableSearchModel GroupsSearchModel = new GroupsSearchModel();

    [NotNull]
    private List<Address>? Addresses { get; set; }

    [NotNull]
    private List<SelectedItem>? ParamentGroups { get; set; } = [];

    #endregion

    private string? AddModalTitle;
    private string? EditModalTitle;

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        //Targets = LookupHelper.GetTargets();
        //Apps = DictService.GetApps().ToSelectedItemList();
        AddModalTitle = Localizer["AddModalTitle"];
        EditModalTitle = Localizer["EditModalTitle"];

        //ParamentGroups = GroupService.GetAll().Where(g => g.ParentId == "0").Select(s => new SelectedItem(s.Id!, s.GroupName)).ToList();
        LoadParamentMenus();

        base.OnInitialized();
    }

    /// <summary>
    /// [覆寫方法] [非同步] 元件初始化
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        Addresses = await AddressService.GetAllAsync();
        await base.OnInitializedAsync();
    }

    private async Task OnAssignmentUsers(Group group)
    {
        var users = UserService.GetAll().ToSelectedItemList();
        var values = UserService.GetUsersByGroupId(group.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentUsersDialog"]}{group}", users, values, () =>
        {
            var ret = UserService.SaveUsersByGroupId(group.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentRoles(Group group)
    {
        var users = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByGroupId(group.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentRolesDialog"]}{group}", users, values, () =>
        {
            var ret = RoleService.SaveRolesByGroupId(group.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private void LoadParamentMenus()
    {
        ParamentGroups.Clear();
        ParamentGroups = GroupService.GetAll()
            .Select(s => new SelectedItem(s.Id!, $"({s.GroupCode}) {s.GroupName}"))
            .ToList();
        ParamentGroups.Insert(0, new SelectedItem("0", Localizer["SelectText"]));
    }

    private Task<QueryData<Group>> OnQueryAsync(QueryPageOptions options)
    {
        var items = GroupService.GetAll()
            .Where(i => i.ParentId == "0");  //只顯示第一層資料

        // 處理模糊查詢
        if (options.Searches.Any())
        {
            items = items.Where(options.Searches.GetFilterFunc<Group>(FilterLogic.Or));
        }

        //  處理 Filter 高級搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<Group>());
        }

        return Task.FromResult(new QueryData<Group>()
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
        LoadParamentMenus();
        return Task.CompletedTask;
    }

    private async Task<bool> OnDeleteAsync(IEnumerable<Group> groups)
    {
        string? PGName = Localizer["DelLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = "D";
        string loginIP = (await WebClientService.GetClientInfo()).Ip!;

        var ret = await GroupService.DeleteGroups(groups, AppContext.UserName, chgType, PGName, PGCode, loginIP);
        LoadParamentMenus();
        return ret;
    }

    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="group"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    private async Task<bool> OnSaveAsync(Group group, ItemChangedType changedType)
    {
        if (group == null) return false;

        string? PGName = changedType == ItemChangedType.Add ? Localizer["AddLogPGName"] : Localizer["EditLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = changedType == ItemChangedType.Add ? "I" : "U";
        string loginIP = (await WebClientService.GetClientInfo()).Ip!;

        var ret = await CommonService.SaveAsync(group, (BootstrapAdmin.DataAccess.Models.ItemChangedTypeAction)changedType, AppContext.UserName, chgType, PGName, PGCode, loginIP);

        return ret;
    }

    private Task<IEnumerable<TableTreeNode<Group>>> TreeNodeConverter(IEnumerable<Group> items)
    {
        // 構造樹狀資料結構
        var ret = BuildTreeNodes(items, "0");
        return Task.FromResult(ret);

        IEnumerable<TableTreeNode<Group>> BuildTreeNodes(IEnumerable<Group> items, string parentId)
        {
            var ret = new List<TableTreeNode<Group>>();
            ret.AddRange(items.Where(i => i.ParentId == parentId).Select((group, index) => new TableTreeNode<Group>(group)
            {
                // 是否有下屬單位
                HasChildren = GroupService.HasChildren(group.Id),  //items.Any(i => i.ParentId == group.Id),

                // 如果子項集合有值 則預設展開此節點, 改為默認不展開
                //IsExpand = items.Any(i => i.ParentId == group.Id),

                // 獲得子項集合, 改為點擊時再從資料庫取得
                //Items = BuildTreeNodes(items.Where(i => i.ParentId == group.Id), group.Id!)
            }));
            return ret;
        }
    }

    private Task<IEnumerable<TableTreeNode<Group>>> OnTreeExpand(Group group)
    {
        return Task.FromResult(GroupService.GetAll().Where(i => i.ParentId == group.Id).AsEnumerable().Select(i => new TableTreeNode<Group>(i)
        {
            //是否有下屬單位
            HasChildren = GroupService.HasChildren(i.Id),  //items.Any(i => i.ParentId == group.Id),

        }));
    }

    /// <summary>
    /// Read groups
    /// </summary>
    /// <returns></returns>
    private List<Group> GenerateGroups(string groupId) =>
         GroupService.GetAll().Where(i => i.ParentId == groupId).ToList();

    private bool ModelEqualityComparer(Group x, Group y) => x.Id == y.Id;
}
