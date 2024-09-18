// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Pages.Admin;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.Web.Validators;

/// <summary>
/// 
/// </summary>
public class AppIdValidator : IValidator
{
    private IDict DictService { get; set; }

    private IStringLocalizer<AppIdValidator> Localizer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dictService"></param>
    /// <param name="localizer"></param>
    public AppIdValidator(IDict dictService, IStringLocalizer<AppIdValidator> localizer) {
        DictService = dictService;
        Localizer = localizer;
    } 

    /// <summary>
    /// 
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyValue"></param>
    /// <param name="context"></param>
    /// <param name="results"></param>
    public void Validate(object? propertyValue, ValidationContext context, List<ValidationResult> results)
    {
        var check = DictService.ExistsAppId(propertyValue?.ToString()!);
        if (check)
        {
            ErrorMessage = $"{context.DisplayName}{Localizer["Existed"]}";
        }
        else
        {
            ErrorMessage = null;
        }
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            results.Add(new ValidationResult(ErrorMessage, new string[] { context.MemberName! }));
        }
    }
}
