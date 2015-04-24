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
      MainWindow = new HostWindow();
      var kernel = new StandardKernel(new DefaultModule(MainWindow as HostWindow));
      
      // Configuration
      var cfg = kernel.Get<AppConfiguration>() as DesktopConfiguration;
      if (cfg == null) throw new Exception("Can not create application configuration.");
      cfg.InitConfigurationFolder();
      await cfg.LoadAsync();
      
      // Appearance
      SetAccentAndBackground(cfg);
      await SetCulture(cfg);
      
      // HostWindowModel
      var hostWindowModel = kernel.Get<HostWindowModel>();
      MainWindow.DataContext = hostWindowModel;
      MainWindow.Show();
      await hostWindowModel.InitializeAsync(null);

      await kernel.Get<INavigationService>().NavigateAsync<BootstrapperPageModel>();
    }

    private void SetAccentAndBackground(DesktopConfiguration cfg)
    {
      AppearanceManager.Current.AccentColor = cfg.AccentColor.ToColor();
      try
      {
        cfg.SetBackground(cfg.BackgroundImage);
      }
      catch
      {
        cfg.SetBackground(cfg.Constants.DefaultBackgroundImage);
      }
    }

    private async Task SetCulture(DesktopConfiguration cfg)
    {
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
    }
  }

  
}
