﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface ISendEmail
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    List<SendEmail> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<SendEmail?> GetSendEmailAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SendEmail();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<SendEmail?> GetByIdAsync(string? Id);
}
