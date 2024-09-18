// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class ApLogSearchModel : ITableSearchModel
{
    /// <summary>
    /// 登錄時間
    /// </summary>
    [NotNull]
    public DateTimeRangeValue? ChgTime { get; set; }

    /// <summary>
    /// 請求IP
    /// </summary>
    public string? ChgIP { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ApLogSearchModel()
    {
        Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFilterAction> GetSearches()
    {
        var ret = new List<IFilterAction>();

        if (!string.IsNullOrEmpty(ChgIP))
        {
            ret.Add(new SearchFilterAction(nameof(ApLog.ChgIP), ChgIP));
        }

        if (ChgTime != null)
        {
            //1121128 先移除，日志查詢會有問題
            ret.Add(new SearchFilterAction(nameof(ApLog.ChgTime), ChgTime.Start, FilterAction.GreaterThanOrEqual));
            ret.Add(new SearchFilterAction(nameof(ApLog.ChgTime), ChgTime.End, FilterAction.LessThanOrEqual));
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        ChgTime = new DateTimeRangeValue
        {
            Start = DateTime.Now.AddMonths(-6),
            End = DateTime.Now
        };
        ChgIP = null;
    }
}
