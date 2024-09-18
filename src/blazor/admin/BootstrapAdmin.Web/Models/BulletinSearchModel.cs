// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.News;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class BulletinSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? startDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? endDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFilterAction> GetSearches()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(Title))
            {
                ret.Add(new SearchFilterAction(nameof(Bulletin.Title), Title));
            }
            if (!string.IsNullOrEmpty(State))
            {
                ret.Add(new SearchFilterAction(nameof(Bulletin.IsPubText), State));
            }


            if (startDate is not null)
            {
                ret.Add(new SearchFilterAction(nameof(Bulletin.StartDate), startDate, FilterAction.GreaterThanOrEqual));
            }

            if (endDate is not null)
            {
                ret.Add(new SearchFilterAction(nameof(Bulletin.EndDate), endDate, FilterAction.LessThanOrEqual));
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            Title = null;
            Content = null;
            State = null;
            State = null;
            startDate = null;
            endDate = null;
        }
    }
}
