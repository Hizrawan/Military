// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class FormDetail
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public Form? Value { get; set; }
         
    [Inject]
    [NotNull]
    private IForm? FormService { get; set; }
     
    [Inject]
    [NotNull]
    private IWebHostEnvironment? WebHost { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<FormDetail>? Localizer { get; set; }

    private string FileFolder => $"upload/form/{Value.Id}/";

    private List<string>? FileNames { get; set; }

    private List<UploadFile> PreviewFileList { get; set; } = new();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        LoadFile(Value.Id);
    }

    private void LoadFile(string? Id)
    {
        if (string.IsNullOrEmpty(Id)) return;
        if (!int.TryParse(Id, out _)) return;

        FileNames = FormService.GetFileNames(Id);

        //empty record
        if (!(FileNames?.Any() ?? false)) return;

        foreach (var FileName in FileNames)
        {
            var prevUrl = Path.Combine(FileFolder, FileName);  //preview url  ex: upload/Bulletin/1  => wwwroot/upload/Bulletin/1
            var fileNamePath = WebHost.CombineTypeNameFile(FileFolder, FileName);
            if (File.Exists(fileNamePath))
            {
                var uploadFile = new UploadFile()
                {
                    FileName = FileName,
                    PrevUrl = prevUrl
                };
                var fi = new FileInfo(fileNamePath);
                uploadFile.Size = fi.Length;
                PreviewFileList.Add(uploadFile);
            }
        }
    }

}
