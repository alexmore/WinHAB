using System;
using System.IO;
using Ninject;
using Ninject.Modules;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Desktop.ViewModels;
using WinHAB.Desktop.Views;

namespace WinHAB.Desktop.Configuration
{
  public class DefaultModule : NinjectModule
  {
    private readonly HostWindow _hostWindow;

    public DefaultModule(HostWindow hostWindow)
    {
      _hostWindow = hostWindow;
    }

    public override void Load()
    {
      Bind<HostWindow>().ToConstant(_hostWindow);
      Bind<INavigationHost>().ToConstant(_hostWindow).InSingletonScope();

      Bind<string>().ToConstant(AppConstants.ConfigurationFile).WhenInjectedInto<JsonConfigurationProvider>();
      Bind<IConfigurationProvider>().To<JsonConfigurationProvider>();
      Bind<AppConfiguration>().To<DesktopConfiguration>().InSingletonScope();


      Bind<string>().ToMethod(x => GetServerAddress(x.Kernel)).WhenInjectedInto<RestClientFactory>();
      Bind<IRestClient>().To<RestClient>();
      Bind<IRestClientFactory>().To<RestClientFactory>();
      
      var vmvFactory = Kernel.Get<DesktopViewModelViewFactory>();
      ConfigureVMVFactory(vmvFactory);
      Bind<MainViewModel>().To<DesktopMainViewModel>();
      Bind<IViewModelViewFactory>().ToConstant(vmvFactory).InSingletonScope();
      Bind<INavigationService>().To<DesktopNavigationService>().InSingletonScope();
    }

    void ConfigureVMVFactory(IViewModelViewFactory f)
    {
      f.Map<LaunchViewModel, LaunchView>();
      f.Map<MainViewModel, MainView>(); 
      f.Map<DesktopMainViewModel, MainView>();
    }

    string GetServerAddress(IKernel kernel)
    {
      try
      {
        var cfg = kernel.Get<AppConfiguration>();
        if (cfg != null && !string.IsNullOrWhiteSpace(cfg.Server))
          return cfg.Server;
      }
      catch (Exception)
      {
      }

      return string.Empty;
    }
  }
}