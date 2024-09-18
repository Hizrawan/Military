// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 字典維護自訂高級搜索模型
    /// </summary>
    public class ErrorSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 獲得/設置 異常類型
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 獲得/設置 使用者名
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 獲得/設置 請求網址
        /// </summary>
        public string? ErrorPage { get; set; }

        /// <summary>
        /// 獲得/設置 記錄時間
        /// </summary>
        [NotNull]
        public DateTimeRangeValue? LogTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ErrorSearchModel()
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

            if (!string.IsNullOrEmpty(Category))
            {
                ret.Add(new SearchFilterAction(nameof(Error.Category), Category));
            }

            if (!string.IsNullOrEmpty(ErrorPage))
            {
                ret.Add(new SearchFilterAction(nameof(Error.ErrorPage), ErrorPage));
            }

            if (LogTime != null)
            {
                ret.Add(new SearchFilterAction(nameof(Error.LogTime), LogTime.Start, FilterAction.GreaterThanOrEqual));
                ret.Add(new SearchFilterAction(nameof(Error.LogTime), LogTime.End, FilterAction.LessThanOrEqual));
            }

            if (!string.IsNullOrEmpty(UserId))
            {
                ret.Add(new SearchFilterAction(nameof(Error.UserId), UserId));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            Category = null;
            UserId = null;
            ErrorPage = null;
            LogTime = new DateTimeRangeValue
            {
                Start = DateTime.Now.AddDays(-7),
                End = DateTime.Now
            };
        }
    }
}
