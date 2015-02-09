using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Desktop.Configuration;
using CoreCfg = WinHAB.Core.Configuration;

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

      var cfg = kernel.Get<CoreCfg.AppConfiguration>();
      await cfg.LoadAsync();
      
      System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(cfg.Language);
      
      MainWindow.Show();

      var navigation = kernel.Get<INavigationService>();
      navigation.Navigate<LaunchViewModel>();
    }
  }
}
