﻿using System;
using System.IO;
using Ninject;
using Ninject.Modules;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Desktop.Views;

namespace WinHAB.Desktop.Configuration
{
  public class DefaultModule : NinjectModule
  {
    private readonly INavigationHost _navigationHost;

    public DefaultModule(INavigationHost navigationHost)
    {
      _navigationHost = navigationHost;
    }

    public override void Load()
    {
      Bind<INavigationHost>().ToConstant(_navigationHost).InSingletonScope();

      Bind<string>().ToConstant(AppConstants.ConfigurationFile).WhenInjectedInto<JsonConfigurationProvider>();
      Bind<IConfigurationProvider>().To<JsonConfigurationProvider>();
      Bind<AppConfiguration>().ToSelf().InSingletonScope();


      Bind<string>().ToMethod(x => GetServerAddress(x.Kernel)).WhenInjectedInto<RestClientFactory>();
      Bind<IRestClient>().To<RestClient>();
      Bind<IRestClientFactory>().To<RestClientFactory>();
      
      var vmvFactory = Kernel.Get<DesktopViewModelViewFactory>();
      ConfigureVMVFactory(vmvFactory);
      Bind<IViewModelViewFactory>().ToConstant(vmvFactory).InSingletonScope();
      Bind<INavigationService>().To<DesktopNavigationService>().InSingletonScope();
    }

    void ConfigureVMVFactory(IViewModelViewFactory f)
    {
      f.Map<LaunchViewModel, LaunchView>();
      f.Map<MainViewModel, MainView>();
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