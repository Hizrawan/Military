// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class MenusSearchModel : ITableSearchModel
{
    /// <summary>
    /// 名稱
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// 獲得/設置 類別, 0 表示系統功能表 1 表示使用者自訂功能表
    /// </summary>
    public EnumNavigationCategory? Category { get; set; }

    /// <summary>
    /// 獲得/設置 目標
    /// </summary>
    public string? Target { get; set; }

    /// <summary>
    /// 獲得/設置  類型  是否為資源檔, 0 表示選單 1 表示資源 2 表示按鈕
    /// </summary>
    public EnumResource? IsResource { get; set; }

    /// <summary>
    /// 獲得/設置 所屬應用，此屬性由BA後臺字典表分配
    /// </summary>
    public string? Application { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFilterAction> GetSearches()
    {
        var ret = new List<IFilterAction>();
        if (!string.IsNullOrEmpty(Name))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Name), Name, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Url))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Url), Url, FilterAction.Equal));
        }

        if (Category.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Category), Category.Value, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Application))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Application), Application, FilterAction.Equal));
        }

        if (IsResource.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.IsResource), IsResource.Value, FilterAction.Equal));
        }

        if (!string.IsNullOrEmpty(Target))
        {
            ret.Add(new SearchFilterAction(nameof(Navigation.Target), Target, FilterAction.Equal));
        }
        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        Name = null;
        Url = null;
        Category = null;
        IsResource = null;
        Target = null;
        Application = null;
    }
}
