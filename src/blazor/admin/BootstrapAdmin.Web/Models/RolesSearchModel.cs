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
    public class RolesSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 獲得/設置 角色名稱
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 獲得/設置 角色描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFilterAction> GetSearches()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(RoleName))
            {
                ret.Add(new SearchFilterAction(nameof(Role.RoleName), RoleName));
            }

            if (!string.IsNullOrEmpty(Description))
            {
                ret.Add(new SearchFilterAction(nameof(Role.Description), Description));
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            RoleName = null;
            Description = null;
        }
    }
}
