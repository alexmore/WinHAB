using System.Windows.Media;

namespace WinHAB.Desktop.Fx.Windows
{
  public static class ColorExtensions
  {
    public static string ToHexString(this Color c)
    {
      return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);
    }
  }
}