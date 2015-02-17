﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
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

      var ci = new CultureInfo(cfg.Language);
      Thread.CurrentThread.CurrentCulture = ci;
      Thread.CurrentThread.CurrentUICulture = ci;
      
      MainWindow.Show();

      var navigation = kernel.Get<INavigationService>();
      
      var vm = navigation.Navigate<LaunchViewModel>();
      vm.HideAll();
      vm.Waiter.Show(Localizations.Localization.Starting);
      if (!string.IsNullOrWhiteSpace(cfg.Server)) await vm.ConnectCommand.ExecuteAsync(cfg.Server);
      else vm.ShowServerUrl();
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
