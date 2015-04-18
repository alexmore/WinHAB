using System.Globalization;

namespace WinHAB.Core.Fx
{
  public static class DecimalExtensions
  {
    public static decimal? ToDecimal(this string numberString)
    {
      decimal val = 0;
      if (decimal.TryParse(numberString, NumberStyles.Number, CultureInfo.InvariantCulture, out val))
        return val;

      return null;
    }
  }
}