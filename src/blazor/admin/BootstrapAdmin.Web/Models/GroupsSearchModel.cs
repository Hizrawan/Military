// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 角色維護自訂進階搜尋模型
    /// </summary>
    public class GroupsSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 獲得/設置 組織名稱
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// 獲得/設置 組織代碼
        /// </summary>
        public string? GroupCode { get; set; }

        /// <summary>
        /// 獲得/設置 組織說明
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 獲得/設置 組織聯絡電話
        /// </summary>
        public string? ContactTel { get; set; }

        /// <summary>
        /// 獲得/設置 組織聯絡人
        /// </summary>
        public string? ContactPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFilterAction> GetSearches()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(GroupName))
            {
                ret.Add(new SearchFilterAction(nameof(User.UserName), GroupName));
            }

            if (!string.IsNullOrEmpty(GroupCode))
            {
                ret.Add(new SearchFilterAction(nameof(User.DisplayName), GroupCode));
            }

            if (!string.IsNullOrEmpty(Description))
            {
                ret.Add(new SearchFilterAction(nameof(User.DisplayName), Description));
            }

            if (!string.IsNullOrEmpty(ContactTel))
            {
                ret.Add(new SearchFilterAction(nameof(User.DisplayName), ContactTel));
            }

            if (!string.IsNullOrEmpty(ContactPerson))
            {
                ret.Add(new SearchFilterAction(nameof(User.DisplayName), ContactPerson));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            GroupName = null;
            GroupCode = null;
            Description = null;
            ContactTel = null;
            ContactPerson = null;
        }
    }
}
