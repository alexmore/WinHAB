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
    }

    #region Appearance
    
    public string AccentColor
    {
      get { return Provider.Get(this.GetPropertyName(() => AccentColor)); }
      set { Provider.Set(this.GetPropertyName(() => AccentColor), value); }
    }

    public string ThemeSource
    {
      get { return Provider.Get(this.GetPropertyName(() => ThemeSource)); }
      set { Provider.Set(this.GetPropertyName(() => ThemeSource), value); }
    }

    public string BackgroundImagePath
    {
      get { return Provider.Get(this.GetPropertyName(() => BackgroundImagePath)); }
      set { Provider.Set(this.GetPropertyName(() => BackgroundImagePath), value); }
    }

    private readonly KeyValuePair<string, Uri>[] _themes = new []
    {
      new KeyValuePair<string, Uri>(Localization.Strings.LabelThemeDark, AppearanceManager.DarkThemeSource),
      new KeyValuePair<string, Uri>(Localization.Strings.LabelThemeLight, AppearanceManager.LightThemeSource)
    };

    public KeyValuePair<string, Uri>[] Themes
    {
      get { return _themes; }
    }

    private readonly string[] _accentColors = new[] { "#0099CC", "#9933CC", "#669900", "#FF8800", "#CC0000" };
    public string[] AccentColors
    {
      get { return _accentColors; }
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
      
      BackgroundImagePath = imageFileName;
      Save();
    }

    #endregion
  }
}