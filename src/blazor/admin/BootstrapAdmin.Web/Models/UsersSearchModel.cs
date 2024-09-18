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
    public class UsersSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 獲得/設置 系統登錄使用者名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 獲得/設置 使用者顯示名稱
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFilterAction> GetSearches()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(UserName))
            {
                ret.Add(new SearchFilterAction(nameof(User.UserName), UserName));
            }

            if (!string.IsNullOrEmpty(DisplayName))
            {
                ret.Add(new SearchFilterAction(nameof(User.DisplayName), DisplayName));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            UserName = null;
            DisplayName = null;
        }
    }
}
