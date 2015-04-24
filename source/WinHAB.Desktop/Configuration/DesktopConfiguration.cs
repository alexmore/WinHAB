using System;
using System.Collections.Generic;
using System.IO;
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
    private readonly AppConstants _constants;

    public DesktopConfiguration(IConfigurationProvider provider, HostWindow hostWindow, AppConstants constants) : base(provider)
    {
      _hostWindow = hostWindow;
      _constants = constants;
    }

    public AppConstants Constants { get { return _constants; }}

    #region Appearance

    public string[] AccentColors { get { return _constants.AccentColors; } }

    public string AccentColor
    {
      get { return Provider.Get(this.GetPropertyName(() => AccentColor))??_constants.DefaultAccentColor; }
      set { Provider.Set(this.GetPropertyName(() => AccentColor), value); }
    }
    
    public string BackgroundImage
    {
      get { return Provider.Get(this.GetPropertyName(() => BackgroundImage))??_constants.DefaultBackgroundImage; }
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

    public void InitConfigurationFolder()
    {
      if (!Directory.Exists(Constants.ConfigurationFolder))
        Directory.CreateDirectory(Constants.ConfigurationFolder);
    }
  }
}