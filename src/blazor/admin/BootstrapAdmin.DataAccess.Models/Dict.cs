// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 字典配置項
/// </summary>
[Table("Dicts")]
public class Dict
{
    /// <summary>
    /// 獲得/設置 字典主鍵 資料庫自增列
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 字典標籤
    /// </summary>
    [Required]
    public string? Category { get; set; }

    /// <summary>
    /// 獲得/設置 字典名稱
    /// </summary>
    [Required]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 獲得/設置 字典代碼
    /// </summary>
    [Required]
    [NotNull]
    public string? Code { get; set; }    

    /// <summary>
    /// 獲得/設置 字典定義值(字典類型) 0 表示系統使用，1 表示使用者自訂 默認為 1
    /// </summary>
    public EnumDictDefine Define { get; set; } = EnumDictDefine.Customer;

    /// <summary>
    /// 獲得/設置 是否啟用
    /// </summary>
    public bool IsEnable { get; set; }

    /// <summary>
    /// 獲得/設置 是否刪除
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// 獲得/設置 排序
    /// </summary>
    public int ShowOrder { get; set; }

    /// <summary>
    /// 獲得/設置 參數1
    /// </summary>
    public string? param1 { get; set; }

    /// <summary>
    /// 獲得/設置 參數2
    /// </summary>
    public string? param2 { get; set; }

    /// <summary>
    /// 獲得/設置 參數3
    /// </summary>
    public string? param3 { get; set; }

    /// <summary>
    /// 獲得/設置 參數4
    /// </summary>
    public string? param4 { get; set; }

    /// <summary>
    /// 獲得/設置 參數5
    /// </summary>
    public string? param5 { get; set; }

    /// <summary>
    /// 獲得/設置 參數6
    /// </summary>
    public string? param6 { get; set; }

    /// <summary>
    /// 獲得/設置 參數7
    /// </summary>
    public string? param7 { get; set; }

    /// <summary>
    /// 獲得/設置 參數8
    /// </summary>
    public string? param8 { get; set; }

    /// <summary>
    /// 获得/设置 發佈日期
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 获得/设置 發佈者
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 获得/设置 修改日期
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 获得/设置 修改者
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 获得/设置 刪除日期
    /// </summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>
    /// 获得/设置 刪除者
    /// </summary>
    public string? DeleteBy { get; set; }
}
