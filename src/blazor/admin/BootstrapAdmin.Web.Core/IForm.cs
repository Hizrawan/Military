using BootstrapAdmin.DataAccess.Models;

namespace BootstrapAdmin.Web.Core;

/// <summary>
/// 
/// </summary>
public interface IForm
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Form> GetAll();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool SaveFileName(string Id, string? FileName);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool DeleteFileName(string Id, string? FileName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    List<string>? GetFileNames(string Id);
}
