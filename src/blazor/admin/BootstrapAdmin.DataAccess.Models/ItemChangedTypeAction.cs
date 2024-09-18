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
/// 数据变化类型, TODO  因不跨層，因此先不引用 BootstrapBlazor.Webcompoents
/// 因在自定義新增或修改時，需要保存使用者訊息，在自定 OnSaveAsync中使用到ItemChangedType來判斷類別
/// </summary>
public enum ItemChangedTypeAction
{
    /// <summary>
    /// 新建
    /// </summary>
    Add,

    /// <summary>
    /// 更新
    /// </summary>
    Update,
}
