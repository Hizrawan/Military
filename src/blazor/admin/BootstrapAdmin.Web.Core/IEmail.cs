// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IEmail
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    Task SendEmailAsync(string to, string subject, string body);
}
