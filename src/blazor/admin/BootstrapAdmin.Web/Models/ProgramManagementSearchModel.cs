// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models.AdminPages.PermissionControl;

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProgramManagementSearchModel : ITableSearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? ProgramName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? ProgramPath { get; set; }


        public IEnumerable<IFilterAction> GetSearches()
        {
            var ret = new List<IFilterAction>();

            if (!string.IsNullOrEmpty(ProgramName))
            {
                ret.Add(new SearchFilterAction(nameof(ProgramManagement.ProgramName), ProgramName));
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Reset()
        {
            ProgramName = null;
            ProgramPath = null;

        }
    }
}
