using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using Ninject;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.ViewModels;
using WinHAB.Desktop.Windows;

namespace WinHAB.Desktop
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
      if (!Directory.Exists(AppConstants.ConfigurationFolder))
        Directory.CreateDirectory(AppConstants.ConfigurationFolder);

      MainWindow = new HostWindow();
      
      var kernel = new StandardKernel(new DefaultModule(MainWindow as HostWindow));

      var cfg = kernel.Get<AppConfiguration>() as DesktopConfiguration;
      await cfg.LoadAsync();

      // Appearance settings
      SetAppearance(cfg);
     
      System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(cfg.Language);
      
      MainWindow.Show();

      var navigation = kernel.Get<INavigationService>();
      //navigation.Navigate<LaunchViewModel>();
      navigation.Navigate<DesktopMainViewModel>();
    }

    private void SetAppearance(DesktopConfiguration cfg)
    {
      try
      {
        AppearanceManager.Current.AccentColor = (Color) ColorConverter.ConvertFromString(cfg.AccentColor);
      }
      catch (Exception)
      {
        cfg.AccentColor = AppConstants.DefaultAccentColor.ToHexString();
        cfg.Save();
        AppearanceManager.Current.AccentColor = AppConstants.DefaultAccentColor;
      }

      try
      {
        AppearanceManager.Current.ThemeSource = new Uri(cfg.ThemeSource, UriKind.Relative);
      }
      catch (Exception)
      {
        cfg.ThemeSource = AppConstants.DefaultThemeSource.OriginalString;
        cfg.Save();
        AppearanceManager.Current.ThemeSource = AppConstants.DefaultThemeSource;
      }

      try
      {
        cfg.SetBackground(cfg.BackgroundImagePath);
      }
      catch (Exception)
      {
      }
    }
  }

  
}
