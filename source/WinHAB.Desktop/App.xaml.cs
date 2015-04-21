using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Converters;
using Ninject;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;
using WinHAB.Desktop.ViewModels;
using WinHAB.Desktop.ViewModels.Pages;

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

      var ci = new CultureInfo(cfg.Language);
      Thread.CurrentThread.CurrentCulture = ci;
      Thread.CurrentThread.CurrentUICulture = ci;


      MainWindow.DataContext = kernel.Get<DesktopMainPageModel>();
      MainWindow.Show();

      var navigation = kernel.Get<INavigationService>();
      await navigation.NavigateAsync<BootstrapperPageModel>();
    }

    private void SetAppearance(DesktopConfiguration cfg)
    {
      if (cfg.AccentColor.IsNullOrWhitespace() || cfg.AccentColor.ToColor().ToHexString() == "#00000000")
      {
        cfg.AccentColor = AppConstants.DefaultAccentColor.ToHexString();
        cfg.Save();
      }

      AppearanceManager.Current.AccentColor = cfg.AccentColor.ToColor();
    
      try
      {
        cfg.SetBackground(cfg.BackgroundImage);
      }
      catch (Exception)
      {
      }
    }
  }

  
}
