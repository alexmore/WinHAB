using System;
using System.IO;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;

namespace WinHAB.Desktop.Configuration
{
  public static class AppConstants
  {
    public static string ConfigurationFolder
    {
      get
      {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinHAB"); 
      }
    }

    public static string ConfigurationFile {
      get { return Path.Combine(ConfigurationFolder, "WinHAB.json"); }
    }

    public static Color DefaultAccentColor { get { return (Color) ColorConverter.ConvertFromString("#FF8800"); }}
    public static Uri DefaultThemeSource { get { return AppearanceManager.LightThemeSource; }}
  }
}