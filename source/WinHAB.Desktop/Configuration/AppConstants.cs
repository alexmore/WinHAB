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
  public class AppConstants
  {
    public string ConfigurationFolder
    {
      get
      {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinHAB"); 
      }
    }

    public string ConfigurationFile {
      get { return Path.Combine(ConfigurationFolder, "WinHAB.json"); }
    }

    public string CustomIconsFile
    {
      get { return Path.Combine(ConfigurationFolder, "CustomIcons.json"); }
    }

    public string DefaultAccentColor { get { return "#ff6600"; } } // Orange

    public string[] AccentColors
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
    public string DefaultBackgroundImage { get { return "pack://application:,,,/Assets/Backgrounds/Background-Town.jpg";}}

    
  }
}