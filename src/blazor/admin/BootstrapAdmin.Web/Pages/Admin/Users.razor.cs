// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

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
public partial class Users
{
    [Inject]
    [NotNull]
    private DialogService? DialogService { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Inject]
    [NotNull]
    private IDict? DictService { get; set; }

    [Inject]
    [NotNull]
    private IGroup? GroupService { get; set; }

    [Inject]
    [NotNull]
    private IRole? RoleService { get; set; }

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    //[Inject]
    //[NotNull]
    //private IVendor? VendorService { get; set; }

    [Inject]
    [NotNull]
    private BootstrapAppContext? AppContext { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<Users>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private WebClientService? WebClientService { get; set; }

    [Inject]
    [NotNull]
    private ICommon<User>? CommonService { get; set; }

    [NotNull]
    private List<SelectedItem>? GroupData { get; set; }

    private List<SelectedItem>? UserTypeSelectItems { get; set; } = new();

    private Dictionary<string, (string Code, string?[] Params)> UserTypeDicts { get; set; } = new();

    private List<SelectedItem>? VendorIDData { get; set; } = new();

    private ITableSearchModel UsersSearchModel { get; } = new UsersSearchModel();

    private string? AddModalTitle;
    private string? EditModalTitle;


    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {

        AddModalTitle = Localizer["AddModalTitle"];
        EditModalTitle = Localizer["EditModalTitle"];

        base.OnInitialized();        
    }

    /// <summary>
    /// [覆寫方法] [非同步] 元件初始化
    /// </summary>
    /// <returns></returns>
    protected override async Task OnInitializedAsync()
    {
        UserTypeDicts = await DictService.GetKeyValueByCategoryAsync(nameof(User.UserType));

        //VendorIDData = (await VendorService.GetAllAsync())
        //    .Where(ven => ven.IsEnable)
        //    .Select(ven => new SelectedItem(ven.Id!, $"({ven.VendorNo}) {ven.VendorName}"))
        //    .ToList();
        //VendorIDData.Insert(0, new(string.Empty, Localizer["VendorNoSelect"]));

        GroupData = GroupService.GetAll()
            .Where(group => group.IsEnable)
            .Select(group => new SelectedItem(group.Id!, $"({group.GroupCode}) {group.GroupName}"))
            .ToList();

        UserTypeSelectItems = UserTypeDicts
            .Select(kvp => new SelectedItem(kvp.Value.Code, kvp.Key))
            .ToList();

        await base.OnInitializedAsync();
    }

    private async Task OnAssignmentGroups(User user)
    {
        var groups = GroupService.GetAll()
            .ToSelectedItemList();

        var values = GroupService.GetGroupsByUserId(user.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentGroupsDialog"]}{user}", groups, values, () =>
        {
            var ret = GroupService.SaveGroupsByUserId(user.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task OnAssignmentRoles(User user)
    {
        var groups = RoleService.GetAll().ToSelectedItemList();
        var values = RoleService.GetRolesByUserId(user.Id);

        await DialogService.ShowAssignmentDialog($"{Localizer["AssignmentRolesDialog"]}{user}", groups, values, () =>
        {
            var ret = RoleService.SaveRolesByUserId(user.Id, values);
            return Task.FromResult(ret);
        }, ToastService);
    }

    private async Task<bool> OnSaveAsync(User user, ItemChangedType itemChangedType)
    {
        // 身分為廠商
        if ($"{UserTypeDicts.Values.FirstOrDefault(value => $"{value.Code}".Equals(user.UserType)).Params[0]}".Equals("Y"))
        {
            if (user.VendorID.GetValueOrDefault() <= 0)
            {
                // 增加失敗顯示吐司提示視窗
                await ToastService.Show(new ToastOption
                {
                    Title = Localizer["SaveErrorText"],
                    Content = Localizer["VendorRequiredVendorId"],
                    Category = ToastCategory.Error
                });
                return false;
            }
        }
        // 身分為其他（此為一般使用者）
        else
        {
            // 一般使用者必填身分證字號
            if (string.IsNullOrWhiteSpace(user.UserSID))
            {
                await ToastService.Show(new ToastOption
                {
                    Title = Localizer["SaveErrorText"],
                    Content = Localizer["NormalUserRequiredUserSID"],
                    Category = ToastCategory.Error
                });
                return false;
            }
            user.VendorID = null;
        }

        // 新增時必須輸入密碼，修改時可不用輸入密碼
        if (string.IsNullOrEmpty(user.Id) && string.IsNullOrEmpty(user.NewPassword))
        {
            await ToastService.Show(new ToastOption
            {
                Title = Localizer["SaveErrorText"],
                Content = Localizer["MustInputPasswordText"],
                Category = ToastCategory.Error
            });
            return false;
        }

        // 驗證身分證字號是否符合規則，若廠商有填寫身分證字號，仍然需要驗證
        if (!string.IsNullOrEmpty(user.UserSID) && !IsValidTaiwanID(user.UserSID))
        {
            await ToastService.Show(new ToastOption
            {
                Title = Localizer["SaveErrorText"],
                Content = Localizer["UserSIDIsInvalid"],
                Category = ToastCategory.Error
            });
            return false;
        }

        //add log
        string? PGName = itemChangedType == ItemChangedType.Add ? Localizer["AddLogPGName"] : Localizer["EditLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = itemChangedType == ItemChangedType.Add ? "I" : "U";
        string loginIP = (await WebClientService.GetClientInfo()).Ip!;

        return UserService.SaveUser(user.UserName, user.DisplayName, user.NewPassword, user.GroupsID, user.UserSID, user.UserEmail, user.UserTelPhone, user.UserCellPhone, user.IsEnable, user.UserType, user.VendorID, AppContext.UserName, chgType, PGName, PGCode, loginIP);  //Task.FromResult(Service.)
    }

    private Task<QueryData<User>> OnQueryAsync(QueryPageOptions options)
    {
        var items = UserService.GetAll().Where(u => !u.IsDelete);

        // 處理模糊查詢
        if (options.Searches.Any())
        {
            items = items.Where(options.Searches.GetFilterFunc<User>(FilterLogic.Or));
        }

        //  處理 Filter 高級搜索
        if (options.CustomerSearches.Any() || options.Filters.Any())
        {
            items = items.Where(options.CustomerSearches.Concat(options.Filters).GetFilterFunc<User>());
        }

        return Task.FromResult(new QueryData<User>()
        {
            IsFiltered = true,
            IsSearch = true,
            IsSorted = true,
            Items = items
        });
    }

    /// <summary>
    /// 刪除方法
    /// </summary>
    /// <param name="users"></param>
    /// <returns></returns>
    private async Task<bool> OnDeleteAsync(IEnumerable<User> users)
    {
        string? PGName = Localizer["DelLogPGName"]; //功能名稱
        string? PGCode = "";    //功能代碼, 待定
        string chgType = "D";
        string loginIP = (await WebClientService.GetClientInfo()).Ip!;

        //這邊要對刪除后的userName 欄位做處理，因此不使用公共方法
        //var ret = await CommonService.DeleteAsync(users, AppContext.UserName, chgType, PGName, PGCode, loginIP);
        //return ret;

        // 這邊不做實際刪除
        var ret = UserService.DeleteUsers(users, AppContext.UserName, chgType, PGName, PGCode, loginIP);
        return ret;
    }

    private bool IsValidTaiwanID(string id)
    {
        if (id?.Length != 10)
        {
            return false;
        }

        int total = 0;
        Dictionary<char, int> cityCodes = new Dictionary<char, int>
        {
            { 'A', 10 },
            { 'B', 11 },
            { 'C', 12 },
            { 'D', 13 },
            { 'E', 14 },
            { 'F', 15 },
            { 'G', 16 },
            { 'H', 17 },
            { 'I', 34 },
            { 'J', 18 },
            { 'K', 19 },
            { 'L', 20 },
            { 'M', 21 },
            { 'N', 22 },
            { 'O', 35 },
            { 'P', 23 },
            { 'Q', 24 },
            { 'R', 25 },
            { 'S', 26 },
            { 'T', 27 },
            { 'U', 28 },
            { 'V', 29 },
            { 'W', 32 },
            { 'X', 30 },
            { 'Y', 31 },
            { 'Z', 33 }
        };

        if (id != null)
        {
            // Convert the first letter to the corresponding value
            char firstChar = char.ToUpper(id[0]);
            cityCodes.TryGetValue(firstChar, out int firstValue);
            string taiwanId = String.Concat(firstValue, id.Substring(1));

            // Multiply each digit by its respective weight
            for (int i = 1; i < 12; i++)
            {
                int weight = 0;
                if (i == 11 || i == 1)
                {
                    weight = 1;
                }
                else
                {
                    weight = 11 - i; // Weight: 9, 8, 7, ..., 1
                }
                int digit = taiwanId[i - 1] - '0'; // Convert character to integer

                total += digit * weight;
            }

            // Check if the checksum is a multiple of 10
            return (total % 10) == 0;
        }
        return true;
    }
}
