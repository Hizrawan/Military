// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 
/// </summary>
public class User
{
    /// <summary>
    /// 獲得/設置 系統登錄使用者名
    /// </summary>
    [Required]
    [RegularExpression("^[a-zA-Z0-9_@.]*$")]
    [MaxLength(16)]
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 獲得/設置 使用者顯示名稱
    /// </summary>
    [Required]
    [MaxLength(20)]
    [NotNull]
    public string? DisplayName { get; set; }

    /// <summary>
    /// 獲得/設置 使用者頭像圖示路徑
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 獲得/設置 使用者設置樣式表名稱
    /// </summary>
    public string? Css { get; set; }

    /// <summary>
    /// 獲得/設置 使用者預設登錄 App 標識
    /// </summary>
    [NotNull]
    public string? App { get; set; }

    /// <summary>
    /// 獲得/設置 使用者主鍵ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲取/設置 密碼
    /// </summary>
    [Required]
    [MaxLength(16)]
    [NotNull]
    public string? Password { get; set; }

    /// <summary>
    /// 獲取/設置 密碼鹽
    /// </summary>
    public string? PassSalt { get; set; }

    /// <summary>
    /// 獲得/設置 使用者註冊時間
    /// </summary>
    public DateTime RegisterTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 獲得/設置 使用者被批復時間
    /// </summary>
    public DateTime? ApprovedTime { get; set; }

    /// <summary>
    /// 獲得/設置 使用者批復人
    /// </summary>
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// 獲得/設置 使用者的申請理由
    /// </summary>
    [NotNull]
    public string? Description { get; set; }

    /// <summary>
    /// 獲得/設置 通知描述 2分鐘內為剛剛
    /// </summary>
    public string? Period { get; set; }

    /// <summary>
    /// 獲得/設置 新密碼
    /// </summary>
    [MaxLength(16)]
    [RegularExpression("^(?![a-zA-Z]+$)(?![A-Z0-9]+$)(?![A-Z\\W_]+$)(?![a-z0-9]+$)(?![a-z\\W_]+$)(?![0-9\\W_]+$)[a-zA-Z0-9\\W_]{8,}$")]
    [NotNull]
    public string? NewPassword { get; set; }

    /// <summary>
    /// 獲得/設置 新密碼
    /// </summary>
    [CompareUserPassword("NewPassword")]
    [RegularExpression("^(?![a-zA-Z]+$)(?![A-Z0-9]+$)(?![A-Z\\W_]+$)(?![a-z0-9]+$)(?![a-z\\W_]+$)(?![0-9\\W_]+$)[a-zA-Z0-9\\W_]{8,}$")]
    [MaxLength(16)]
    [NotNull]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// 獲得/設置 是否重置密碼
    /// </summary>
    public int IsReset { get; set; }

    /// <summary>
    /// 獲得/設置 默認格式為 DisplayName (UserName)
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{DisplayName} ({UserName})";

    /// <summary>
    /// 獲得/設置 使用者組織ID
    /// </summary>
    public int GroupsID { get; set; }

    /// <summary>
    /// 獲得/設置 使用者身分證號
    /// </summary>
    public string? UserSID { get; set; }

    /// <summary>
    /// 獲得/設置 使用者Email 
    /// </summary>
    [RegularExpression("^[A-Za-z0-9\\u4e00-\\u9fa5]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$")]
    public string? UserEmail { get; set; }

    /// <summary>
    /// 獲得/設置 使用者聯絡電話
    /// </summary>
    public string? UserTelPhone { get; set; }

    /// <summary>
    /// 獲得/設置 使用者聯絡手機
    /// </summary>
    public string? UserCellPhone { get; set; }

    /// <summary>
    /// 獲得/設置 是否啟用(0:停用 1:啟用)
    /// </summary>
    public bool IsEnable { get; set; } = false;

    /// <summary>
    /// 獲得/設置 是否刪除
    /// </summary>
    public bool IsDelete { get; set; } = false;

    /// <summary>
    /// 獲得/設置 建立人
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 獲得/設置 建立時間
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 獲得/設置 最後修改人
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 獲得/設置 最後修改時間
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 獲得/設置 刪除人
    /// </summary>
    public string? DeleteBy { get; set; }

    /// <summary>
    /// 獲得/設置 刪除時間
    /// </summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>
    /// 獲得/設置 最後一次登入時間
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// 獲得/設置 最後一次登入IP
    /// </summary>
    public string? LastLoginIP { get; set; }

    /// <summary>
    /// 獲得/設置 密碼錯誤次數
    /// </summary>
    public int PsdErrCount { get; set; }

    /// <summary>
    /// 獲得/設置 是否為最高權限使用者（0：否、1：是）
    /// </summary>
    public bool SuperStatus { get; set; }

    /// <summary>
    /// 獲得/設置 使用者類別
    /// </summary>
    public string? UserType { get; set; }

    // 非資料庫欄位

    /// <summary>
    /// 獲得/設置 使用者類別的值
    /// </summary>
    public string? UserTypeValue { get; set; }

    // End 非資料庫欄位

    /// <summary>
    /// 獲得/設置 廠商ID
    /// </summary>
    public int? VendorID { get; set; }

    // 非資料庫欄位

    /// <summary>
    /// 獲得/設置 廠商名稱
    /// </summary>
    public string? VendorName { get; set; }

    // End 非資料庫欄位
}

/// <summary>
/// 使用者狀態枚舉類型
/// </summary>
public enum UserStates
{
    /// <summary>
    /// 更改密碼
    /// </summary>
    ChangePassword,

    /// <summary>
    /// 更改樣式
    /// </summary>
    ChangeTheme,

    /// <summary>
    /// 更改顯示名稱
    /// </summary>
    ChangeDisplayName,

    /// <summary>
    /// 審批使用者
    /// </summary>
    ApproveUser,

    /// <summary>
    /// 拒絕使用者
    /// </summary>
    RejectUser,

    /// <summary>
    /// 保存默認應用
    /// </summary>
    SaveApp
}
