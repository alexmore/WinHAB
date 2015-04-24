using System;
using System.IO;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Fx;
using WinHAB.Desktop.Fx.Mvvm;
using WinHAB.Desktop.Views;

namespace WinHAB.Desktop.Configuration
{
  public class DefaultModule : NinjectModule
  {
    private readonly HostWindow _hostWindow;
    private readonly AppConstants _constants = new AppConstants();

    public DefaultModule(HostWindow hostWindow)
    {
      _hostWindow = hostWindow;
    }

    public override void Load()
    {
      Bind<AppConstants>().ToConstant(_constants).InSingletonScope();

      Bind<ITimer>().To<DesktopTimer>();

      Bind<HostWindow>().ToConstant(_hostWindow);
      Bind<INavigationHost>().ToConstant(_hostWindow).InSingletonScope();

      Bind<string>().ToConstant(_constants.ConfigurationFile).WhenInjectedInto<JsonConfigurationProvider>();
      Bind<IConfigurationProvider>().To<JsonConfigurationProvider>();
      var config = Kernel.Get<DesktopConfiguration>();
      Bind<AppConfiguration>().ToConstant(config);
      Bind<DesktopConfiguration>().ToConstant(config);


      Bind<string>().ToMethod(x => GetServerAddress(x.Kernel)).WhenInjectedInto<RestClientFactory>();
      Bind<IRestClient>().To<RestClient>();
      Bind<IRestClientFactory>().To<RestClientFactory>();

      var viewFactory = Kernel.Get<DesktopViewFactory>();
      viewFactory.ScanAssembly(this.GetType().Assembly);
      Bind<IViewFactory>().ToConstant(viewFactory).InSingletonScope();
      Bind<INavigationService>().To<DesktopNavigationService>().InSingletonScope();

      Bind<Func<Type, Widget, WidgetModelBase>>()
        .ToMethod(x => (t, d) => (WidgetModelBase) x.Kernel.Get(t, new ConstructorArgument("data", d)));
      Bind<IWidgetsFactory>().To<WidgetsFactory>();
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