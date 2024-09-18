using System.ComponentModel.DataAnnotations;

namespace BootstrapAdmin.DataAccess.Models;
/// <summary>
/// 比較新密碼和確認密碼是否相同
/// 條件：兩者有輸入其中一項時才比較
/// </summary>
public class CompareUserPasswordAttribute : CompareAttribute
{
    private readonly string _other;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="otherProperty"></param>
    public CompareUserPasswordAttribute(string otherProperty) : base(otherProperty)
    {
        _other = otherProperty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_other);
        if (property == null)
        {
            return new ValidationResult(
                string.Format("Unknown property: {0}", _other)
            );
        }

        var otherValue = property.GetValue(validationContext.ObjectInstance, null);  //新密碼

        string valString = (value as string ?? String.Empty).Trim();
        string otherPropValString = (otherValue as string ?? String.Empty).Trim();

        //其中有一個輸入時 才會比較
        if (!string.IsNullOrEmpty(valString) || !string.IsNullOrEmpty(otherPropValString))
        {
            //都有輸入且相等
            if (String.Compare(valString, otherPropValString, false) == 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessageString);
        }

        return ValidationResult.Success;
    }
}
