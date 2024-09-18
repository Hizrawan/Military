// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 所有類別的共用方法
/// </summary>
public interface IBase
{

    /// <summary>
    /// 存檔案名稱
    /// </summary>
    /// <param name="Id">對應資料表格的自增ID</param>
    /// <param name="FileName">檔案名稱</param>
    bool SaveFileName(string Id, string? FileName);

    /// <summary>
    /// 刪除檔案
    /// </summary>
    /// <param name="Id">對應資料表格的自增ID</param>
    /// <param name="FileName">檔案名稱</param>
    /// <returns></returns>
    bool DeleteFileName(string Id, string? FileName);

    /// <summary>
    /// 取得所有檔案名稱
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFileNames(string Id);
}
