using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FirstFloor.ModernUI.Presentation;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;

namespace WinHAB.Desktop.Configuration
{
  public class DesktopConfiguration : AppConfiguration
  {
    private HostWindow _hostWindow;

    public DesktopConfiguration(IConfigurationProvider provider, HostWindow hostWindow) : base(provider)
    {
      _hostWindow = hostWindow;

      Themes = new[]
      {
        new KeyValuePair<string, Uri>(Localization.Strings.LabelThemeDark, AppearanceManager.DarkThemeSource),
        new KeyValuePair<string, Uri>(Localization.Strings.LabelThemeLight, AppearanceManager.LightThemeSource)
      };

      AccentColors = new[]
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
       "#FA6800", // Orange
       "#825A2C", // Brown
       "#6D8764", // Olive
       "#647687", // Steel
       "#76608A", // Mauve
       "#87794E", // Taupe
      };
    }

    #region Appearance

    public string[] AccentColors { get; private set; }

    public string AccentColor
    {
      get { return Provider.Get(this.GetPropertyName(() => AccentColor)); }
      set { Provider.Set(this.GetPropertyName(() => AccentColor), value); }
    }

    public KeyValuePair<string, Uri>[] Themes { get; private set; }

    public string Theme
    {
      get { return Provider.Get(this.GetPropertyName(() => Theme)); }
      set { Provider.Set(this.GetPropertyName(() => Theme), value); }
    }

    public string BackgroundImage
    {
      get { return Provider.Get(this.GetPropertyName(() => BackgroundImage)); }
      set { Provider.Set(this.GetPropertyName(() => BackgroundImage), value); }
    }

    public void SetBackground(string imageFileName)
    {
      if (!string.IsNullOrWhiteSpace(imageFileName))
      {

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(imageFileName);
        bitmap.EndInit();

        var img = new Image();
        img.Source = bitmap;
        img.Stretch = Stretch.UniformToFill;

        _hostWindow.BackgroundContent = img;
      }
      else
        _hostWindow.BackgroundContent = null;
      
      BackgroundImage = imageFileName;
      Save();
    }

    #endregion
  }
}