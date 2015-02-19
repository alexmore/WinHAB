using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WinHAB.Core.Fx
{
  public static class ReflectionExtensions
  {
    public static string GetPropertyName<T>(this object obj, Expression<Func<T>> propertyExpression)
    {
      if (propertyExpression == null)
        throw new ArgumentNullException("propertyExpression");
      MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
      if (memberExpression == null)
        throw new ArgumentException("Invalid argument", "propertyExpression");
      PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
      if (propertyInfo == null)
        throw new ArgumentException("Argument is not a property", "propertyExpression");
      else
        return propertyInfo.Name;
    }
  }
}