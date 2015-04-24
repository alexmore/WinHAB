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

    public string[] AccentColors { get { return AppConstants.AccentColors; } }

    public string AccentColor
    {
      get { return Provider.Get(this.GetPropertyName(() => AccentColor))??AppConstants.DefaultAccentColor; }
      set { Provider.Set(this.GetPropertyName(() => AccentColor), value); }
    }
    
    public string BackgroundImage
    {
      get { return Provider.Get(this.GetPropertyName(() => BackgroundImage))??AppConstants.DefaultBackgroundImage; }
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
    }

    #endregion
  }
}