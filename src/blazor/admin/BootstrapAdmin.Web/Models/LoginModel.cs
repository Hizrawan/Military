// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Models
{
    /// <summary>
    /// 登錄頁面 Model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 驗證碼圖床地址
        /// </summary>
        public string? ImageLibUrl { get; protected set; }

        /// <summary>
        /// 是否登錄認證失敗 為真時使用者端彈出滑塊驗證碼
        /// </summary>
        public bool AuthFailed { get; set; }
    }
}
