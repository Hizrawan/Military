// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using BootstrapAdmin.DataAccess.Models;
using BootstrapAdmin.Web.Core;
using BootstrapAdmin.Web.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System.Text.Json;
using BootstrapAdmin.DataAccess.PetaPoco.Services;

namespace BootstrapAdmin.Web.Components;

/// <summary>
/// 
/// </summary>
public partial class ApLogDetail
{
    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    [NotNull]
    public ApLog? Value { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<ChgDetail>? ChgDetails { get; set; }

    [Inject]
    [NotNull]
    private IDBManager? DBManager { set; get; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var chgValues = JsonSerializer.Deserialize<List<ChgDetail>>(Value.ChgValue!,
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        using var db = DBManager.Create();
        string sqlScript = $"""
            select  sc.name [Column],sep.value [Description] from sys.tables st
            inner join sys.columns sc on st.object_id = sc.object_id
            left join sys.extended_properties sep on st.object_id = sep.major_id and sc.column_id = sep.minor_id and sep.name = 'MS_Description' 
            where st.name = @0
            """;
        List<TableInfoDetail> tableInfoDetails = db.Fetch<TableInfoDetail>(sqlScript, Value.TableName);

        ChgDetails = from chg in chgValues
                    join tab in tableInfoDetails on chg.ColumnName equals tab.Column
                        select new ChgDetail{ ColumnName = tab.Description, BEFValue = chg.BEFValue, AFTValue = chg.AFTValue };
    }
}

/// <summary>
/// 操作明細
/// </summary>
public class ChgDetail
{
    /// <summary>
    /// 字段名稱
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// 變化前值 
    /// </summary>
    public string? BEFValue { get; set; }

    /// <summary>
    /// 變化后值 
    /// </summary>
    public string? AFTValue { get; set; }
}

/// <summary>
/// 資料表字段及描述說明
/// </summary>
public class TableInfoDetail
{
    /// <summary>
    /// 變化前值 
    /// </summary>
    public string? Column { get; set; }

    /// <summary>
    /// 變化前值 
    /// </summary>
    public string? Description { get; set; }
}

