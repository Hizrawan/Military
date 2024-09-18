using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootstrapAdmin.DataAccess.Models;

/// <summary>
/// 表單管理
/// </summary>
[Table("Forms")]
public class Form
{
    /// <summary>
    /// 获得/设置 主鍵ID
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 获得/设置 表單名稱
    /// </summary>
    [Required]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 表單描述
    /// </summary>
    [NotNull]
    public string? Description { get; set; }

    /// <summary>
    /// 获得/设置 表單分類
    /// </summary>
    [Required]
    [NotNull]
    public string? Category { get; set; }

    /// <summary>
    /// 获得/设置 開始日期
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// 获得/设置 結束日期
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// 获得/设置 是否啟用
    /// </summary>
    public bool IsEnable { get; set; } = false;

    /// <summary>
    /// 获得/设置 是否刪除
    /// </summary>
    public bool IsDelete { get; set; } = false;

    /// <summary>
    /// 获得/设置 下載次數
    /// </summary>
    public int DownloadCount { get; set; } = 0;
    
    /// <summary>
    /// 获得/设置 發佈日期
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 获得/设置 發佈者
    /// </summary>
    public string? CreateBy { get; set; }

    /// <summary>
    /// 获得/设置 修改日期
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// 获得/设置 修改者
    /// </summary>
    public string? ModifyBy { get; set; }

    /// <summary>
    /// 获得/设置 刪除日期
    /// </summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>
    /// 获得/设置 刪除者
    /// </summary>
    public string? DeleteBy { get; set; }
}
