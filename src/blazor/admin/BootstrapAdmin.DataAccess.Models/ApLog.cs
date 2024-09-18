// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the LGPL License, Version 3.0. See License.txt in the project root for license information.
// Website: https://admin.blazor.zone

using System.ComponentModel;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 操作記錄
/// </summary>
public class ApLog
{
    /// <summary>
    /// 獲得/設置 主鍵ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 獲得/設置 表格名稱
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// 獲得/設置 主鍵欄位
    /// </summary>
    public string? PKFIELD { get; set; }

    /// <summary>
    /// 獲得/設置 主鍵值 
    /// </summary>
    public string? PKValue { get; set; }

    /// <summary>
    /// 獲得/設置 功能名稱
    /// </summary>
    public string? PGName { get; set; }

    /// <summary>
    /// 獲得/設置 功能代碼
    /// </summary>
    public string? PGCode { get; set; }

    /// <summary>
    /// 獲得/設置 操作人
    /// </summary>
    public string? UpdBy { get; set; } = "";

    /// <summary>
    /// 獲取/設置 操作類型
    /// </summary>
    public string? ChgType { get; set; }

    /// <summary>
    /// 獲取/設置 操作時間
    /// </summary>
    public DateTime ChgTime { get; set; }

    /// <summary>
    /// 獲取/設置 操作明細
    /// </summary>
    public string? ChgValue { get; set; }

    /// <summary>
    /// 獲取/設置 操作IP
    /// </summary>
    public string? ChgIP { get; set; }

    /// <summary>
    /// 獲取/設置 異動欄位
    /// </summary>
    public string? ChgFIELD { get; set; }

    /// <summary>
    /// 獲取/設置 異動前值 
    /// </summary>
    public string? BEFValue { get; set; }

    /// <summary>
    /// 獲取/設置 異動后值
    /// </summary>
    public string? AFTValue { get; set; }

    
    // 非資料庫欄位 Begin
    /// <summary>
    /// 獲得/設置 操作人
    /// </summary>
    public string? UpdByValue { get; set; }

    /// <summary>
    /// 獲得/設置 異動名稱
    /// </summary>
    public string? ChgTypeValue { get; set; }

    // 非資料庫欄位  End

}

#region LogChgModel Log欄位異動資料模型
/// <summary>
/// 
/// </summary>
public class LogChgModel
{
    /// <summary>
    /// 異動欄位名稱
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// 異動前欄位值
    /// </summary>
    public string? BEFValue { get; set; }

    /// <summary>
    /// 異動後欄位值
    /// </summary>
    public string? AFTValue { get; set; }
}
#endregion
