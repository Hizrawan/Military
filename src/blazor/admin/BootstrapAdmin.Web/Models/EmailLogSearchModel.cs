// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models;

/// <summary>
/// 
/// </summary>
public class EmailLogSearchModel : ITableSearchModel
{
    /// <summary>
    /// 
    /// </summary>
    public EmailLogSearchModel() {
        //初始化只查詢一個星期內的資料
        SendDates = new DateTimeRangeValue
        {
            Start = DateTime.Now.AddDays(-7),
            End = DateTime.Now
        };
    }
  
    /// <summary>
    /// 獲得/設置 郵件類別
    /// </summary>
    public int? EmailType { get; set; }
 
    /// <summary>
    /// 獲得/設置 發送狀態
    /// </summary>
    public int? EmailStatus { get; set; }

    /// <summary>
    /// 獲得/設置 發送日期
    /// </summary>
    [NotNull]
    public DateTimeRangeValue? SendDates { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFilterAction> GetSearches()
    {
        var ret = new List<IFilterAction>();
         
        if (EmailType.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(SendEmail.EmailType), EmailType, FilterAction.Equal));
        }
         
        if (EmailStatus.HasValue)
        {
            ret.Add(new SearchFilterAction(nameof(SendEmail.EmailStatus), EmailStatus, FilterAction.Equal));
        }

        if (SendDates != null)
        {
            ret.Add(new SearchFilterAction(nameof(SendEmail.SendDate), SendDates.Start, FilterAction.GreaterThanOrEqual));
            ret.Add(new SearchFilterAction(nameof(SendEmail.SendDate), SendDates.End, FilterAction.LessThanOrEqual));
        }

        return ret;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        EmailType = null;
        EmailStatus = null;
        SendDates = null;
    }
}
