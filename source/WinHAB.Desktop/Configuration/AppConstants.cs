using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Fx.Windows;

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

    public static string CustomIconsFile
    {
      get { return Path.Combine(ConfigurationFolder, "CustomIcons.json"); }
    }

    public static string DefaultAccentColor { get { return "#ff6600"; } } // Orange

    public static string[] AccentColors
    {
      get
      {
        return new[]
      {
       "#60A917", // Green
       "#008A00", // Emerald
       "#00ABA9", // Teal
       "#1BA1E2", // Cyan
       "#3E65FF", // Cobalt
       "#6A00FF", // Indigo
       "#AA00FF", // Violet
       "#D80073", // Magenta
       "#A20025", // Crimson
       "#E51400", // Red
       "#ff6600", // Orange
       "#825A2C", // Brown
       "#6D8764", // Olive
       "#647687", // Steel
       "#76608A", // Mauve
       "#87794E", // Taupe
      };
      }
    }
    public static string DefaultBackgroundImage { get { return "pack://application:,,,/Assets/Backgrounds/Background-Town.jpg";}}

    #region Widget sizes
    public static Size WidgetMediumSize { get { return new Size(120, 120); } }
    public static double WidgetMediumWidth { get { return WidgetMediumSize.Width; } }
    public static double WidgetMediumHeight { get { return WidgetMediumSize.Height; } }
    
    public static Size WidgetWideSize { get { return new Size(250, 120); } }
    public static double WidgetWideWidth { get { return WidgetWideSize.Width; } }
    public static double WidgetWideHeight { get { return WidgetWideSize.Height; } }
    
    public static Size WidgetLargeSize { get { return new Size(250, 250); } }
    public static double WidgetLargeWidth { get { return WidgetLargeSize.Width; } }
    public static double WidgetLargeHeight { get { return WidgetLargeSize.Height; } }
    
    public static double WidgetMarging { get { return 10; }}
    public static double WidgetMediumFullWidth { get { return WidgetMediumSize.Width + WidgetMarging; } }
    public static double WidgetMediumFullHeight { get { return WidgetMediumSize.Height + WidgetMarging; } }
    
    public static Size GetWidgetSize(WidgetSize size)
    {
      switch (size)
      {
        case WidgetSize.Meduim:
          return WidgetMediumSize;
        case WidgetSize.Wide:
          return WidgetWideSize;
        case WidgetSize.Large:
          return WidgetLargeSize;
        default:
          throw new ArgumentOutOfRangeException("size");
      }
    }
    #endregion
  }
}