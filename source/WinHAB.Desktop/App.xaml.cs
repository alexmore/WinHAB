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
using WinHAB.Desktop.Views.Pages;

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

      if (cfg.Language == null)
      {
        cfg.Language = "Auto";
        await cfg.SaveAsync();
      }

      if (cfg.Language != "Auto")
      {
        var ci = new CultureInfo(cfg.Language);
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
      }

      var hostWindowModel = kernel.Get<HostWindowModel>();
      MainWindow.DataContext = hostWindowModel;
      MainWindow.Show();
      await hostWindowModel.InitializeAsync(null);

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


      if (cfg.BackgroundImage == null)
      {
        cfg.BackgroundImage = "pack://application:,,,/Assets/Backgrounds/Background-WetGlass.jpg";
        cfg.Save();
      }
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
