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
using WinHAB.Desktop.ViewModels;
using WinHAB.Desktop.Views;
using WinHAB.Desktop.Views.WidgetViews;
using WinHAB.Desktop.Windows;

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
      Bind<ITimer>().To<DesktopTimer>();

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
      Bind<MainPage>().To<DesktopMainViewModel>();
      Bind<IViewModelViewFactory>().ToConstant(vmvFactory).InSingletonScope();
      Bind<INavigationService>().To<DesktopNavigationServiceBase>().InSingletonScope();

      Bind<Func<Type, Widget, WidgetBase>>()
        .ToMethod(x => (t, d) => (WidgetBase) x.Kernel.Get(t, new ConstructorArgument("data", d)));
      Bind<WidgetsFactory>().ToSelf();
    }

    void ConfigureVMVFactory(IViewModelViewFactory f)
    {
      f.Map<BootstrapperPage, LaunchView>();
      f.Map<MainPage, MainView>(); 
      f.Map<DesktopMainViewModel, MainView>();
      f.Map<ImageWidgetPage, ImageWidgetPageView>();
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