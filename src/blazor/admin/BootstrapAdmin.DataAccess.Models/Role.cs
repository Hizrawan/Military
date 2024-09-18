// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// Role 實體類
/// </summary>
public class Role
{
    /// <summary>
    /// 獲得/設置 角色主鍵ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 角色名稱
    /// </summary>
    [NotNull]
    public string? RoleName { get; set; }

    /// <summary>
    /// 獲得/設置 角色描述
    /// </summary>
    [NotNull]
    public string? Description { get; set; }

    /// <summary>
    /// 獲得/設置 是否啟用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 獲得/設置 是否刪除
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 獲得/設置 建立者
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 獲得/設置 建立日
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// 獲得/設置 最後修改者
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 獲得/設置 最後修改時間
    /// </summary>
    public DateTime? ModifyDate { get; set; }
}
