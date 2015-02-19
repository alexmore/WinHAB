namespace WinHAB.Core.Fx
{
  public static class StringExtensions
  {
    public static bool IsNullOrWhitespace(this string s)
    {
      return string.IsNullOrWhiteSpace(s);
    }
  }
}