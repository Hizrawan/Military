// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using Microsoft.AspNetCore.Components.Routing;

namespace BootstrapAdmin.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MenuExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public static MenuItem Parse(this DataAccess.Models.Navigation menu) => new()
        {
            Text = menu.Name,
            Url = menu.Url?.Replace("~/", "/"),
            Icon = menu.Icon,
            Match = NavLinkMatch.All,
            Target = menu.Target,
            Id = menu.Id,
            ParentId = menu.ParentId
        };

        /// <summary>
        /// 獲取後臺管理功能表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MenuItem> ToMenus(this IEnumerable<Navigation> navigations)
        {
            var menus = navigations.Where(m => m.Category == EnumNavigationCategory.System && m.IsResource == 0);
            return CascadeMenus(menus);
        }

        /// <summary>
        /// 獲得帶層次關係的功能表集合
        /// </summary>
        /// <param name="navigations">未層次化功能表集合</param>
        /// <returns>帶層次化的功能表集合</returns>
        public static IEnumerable<MenuItem> CascadeMenus(IEnumerable<Navigation> navigations)
        {
            var root = navigations.Where(m => m.ParentId == "0")
                .OrderBy(m => m.Category).ThenBy(m => m.Application).ThenBy(m => m.AppSort).ThenBy(m => m.Order)
                .Select(m => m.Parse())
                .ToList();
            CascadeMenus(navigations, root);
            return root;
        }

        private static void CascadeMenus(IEnumerable<Navigation> navigations, List<MenuItem> level)
        {
            level.ForEach(m =>
            {
                m.Items = navigations.Where(sub => sub.ParentId == m.Id).OrderBy(sub => sub.AppSort).ThenBy(sub => sub.Order).Select(sub => sub.Parse()).ToList();
                CascadeMenus(navigations, m.Items.ToList());
            });
        }
    }
}
