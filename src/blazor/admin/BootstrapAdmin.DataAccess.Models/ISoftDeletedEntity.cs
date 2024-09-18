// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 不實際刪除（當表格中有此欄位時, 則不實際刪除資料, 只更新刪除標記）
/// </summary>
public partial interface ISoftDeletedEntity
{
    /// <summary>
    /// 是否刪除
    /// </summary>
    bool IsDelete { get; set; }
}
