// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Services
{
    /// <summary>
    /// AppContext 實體類
    /// </summary>
    public class BootstrapAppContext
    {
        /// <summary>
        /// 獲得/設置 當前網站 AppId
        /// </summary>
        public string AppId { get; }

        /// <summary>
        /// 獲得/設置 當前登錄帳號
        /// </summary>
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 獲得/設置 當前使用者顯示名稱
        /// </summary>
        [NotNull]
        public string? DisplayName { get; internal set; }

        /// <summary>
        /// 獲得/設置 當前登錄單位
        /// </summary>
        [NotNull]
        public int? GroupId { get; set; }

        /// <summary>
        /// 獲得/設置 當前登錄單位
        /// </summary>
        public int? VendorId { get; set; }

        /// <summary>
        /// 獲得/設置 應用程式基礎位址 如 http://localhost:5210
        /// </summary>
        [NotNull]
        public Uri? BaseUri { get; set; }

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="configuration"></param>
        public BootstrapAppContext(IConfiguration configuration)
        {
            AppId = configuration.GetValue("AppId", "BA")!;
        }
    }
}
