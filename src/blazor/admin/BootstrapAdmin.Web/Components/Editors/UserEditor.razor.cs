// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Validators;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class UserEditor
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public User? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? GroupData { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? UserTypeSelectItems { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public Dictionary<string, (string Code, string?[] Params)> UserTypeDicts { get; set; } = [];

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [EditorRequired]
    [NotNull]
    public List<SelectedItem>? VendorIDData { get; set; } = [];

    [Inject]
    [NotNull]
    private IStringLocalizer<UserEditor>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<UserNameValidator>? LocalizerValidator { get; set; }

    #region -- Service --

    [Inject]
    [NotNull]
    private IUser? UserService { get; set; }

    #endregion

    #region -- Model 和 SelectedItem --

    private List<SelectedItem>? GroupSelectedItems { get; set; } = [];

    private bool GroupSelectIsDisabled { get; set; } = false;

    private List<SelectedItem>? VendorIDSelectedItems { get; set; } = [];

    #endregion

    private List<IValidator> UserRules { get; } = [];

    private bool VendorIDSelectIsVirtualize { get; set; } = true;

    private string? VendorIDSelectNoSearchDataText { get; set; }

    private string? UserNamePlaceholder;
    private string? DisplayNamePlaceholder;
    private string? NewPasswordPlaceholder;
    private string? ConfirmPasswordPlaceholder;

    private static bool GetDisabled(string? id)
        => !string.IsNullOrEmpty(id);

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserRules.Add(new UserNameValidator(UserService, LocalizerValidator));

        UserNamePlaceholder = Localizer["Placeholder", 16];
        DisplayNamePlaceholder = Localizer["Placeholder", 20];
        NewPasswordPlaceholder = Localizer["Placeholder", 16];
        ConfirmPasswordPlaceholder = Localizer["ConfirmPasswordText"];

        if (Value.UserType is null)
        {
            GroupSelectedItems?.AddRange(GroupData);
        }
        else if (Value.UserType.Equals("1"))
        {
            GroupSelectedItems?.Clear();
            GroupSelectedItems?.Add(GroupData.First(item => item.Value == "1"));
            GroupSelectIsDisabled = true;
        }
        else
        {
            GroupSelectedItems?.Clear();
            GroupSelectedItems?.AddRange(GroupData.Where(item => item.Value != "1"));
            GroupSelectIsDisabled = false;
        }
    }

    private Task OnUserTypeSelectedChanged(SelectedItem item)
    {
        List<SelectedItem> empty = [];

        if (item.Value.Equals("1"))
        {
            GroupSelectedItems?.Clear();
            GroupSelectedItems?.Add(GroupData.First(item => item.Value == "1"));
            GroupSelectIsDisabled = true;

            VendorIDSelectedItems?.Clear();
            foreach (var vendorIdDatum in VendorIDData ?? empty)
            {
                VendorIDSelectedItems?.Add(vendorIdDatum);
            }
            (VendorIDSelectIsVirtualize, VendorIDSelectNoSearchDataText) =
                (true, Localizer["VendorIDNoData"]);
        }
        else
        {
            GroupSelectedItems?.Clear();
            GroupSelectedItems?.AddRange(GroupData.Where(item => item.Value != "1"));
            GroupSelectIsDisabled = false;

            VendorIDSelectedItems?.Clear();
            (VendorIDSelectIsVirtualize, VendorIDSelectNoSearchDataText) =
                (false, Localizer["VendorIDNotSelect"]);
        }
        StateHasChanged();
        return Task.CompletedTask;
    }
}
