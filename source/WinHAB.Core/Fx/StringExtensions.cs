using System;

namespace WinHAB.Core.Fx
{
  public static class StringExtensions
  {
    public static bool IsNullOrWhitespace(this string s)
    {
      return string.IsNullOrWhiteSpace(s);
    }

    public static string FormatString(this string format, params object [] args)
    {
      return string.Format(format, args);
    }

    public static string FormatString(this string format, IFormatProvider provider, params object[] args)
    {
      return string.Format(provider, format, args);
    }
  }
}