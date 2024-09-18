// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class LoginLogSearchModel : ITableSearchModel
{
    /// <summary>
    /// 登錄時間
    /// </summary>
    [NotNull]
    public DateTimeRangeValue? LogTime { get; set; }

    /// <summary>
    /// 請求IP
    /// </summary>
    public string? IP { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public LoginLogSearchModel()
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

        if (!string.IsNullOrEmpty(IP))
        {
            ret.Add(new SearchFilterAction(nameof(LoginLog.Ip), IP));
        }

        if (LogTime != null)
        {
            //1121128 先移除，日志查詢會有問題
            ret.Add(new SearchFilterAction(nameof(LoginLog.LoginTime), LogTime.Start, FilterAction.GreaterThanOrEqual));
            ret.Add(new SearchFilterAction(nameof(LoginLog.LoginTime), LogTime.End, FilterAction.LessThanOrEqual));
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        LogTime = new DateTimeRangeValue
        {
            Start = DateTime.Now.AddDays(-7),
            End = DateTime.Now
        };
        IP = null;
    }
}
