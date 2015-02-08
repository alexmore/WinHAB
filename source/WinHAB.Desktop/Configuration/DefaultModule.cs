using System.IO;
using Ninject;
using Ninject.Modules;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;

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
      
      var vmvFactory = Kernel.Get<DesktopViewModelViewFactory>();
      ConfigureVMVFactory(vmvFactory);
      Bind<IViewModelViewFactory>().ToConstant(vmvFactory).InSingletonScope();
      Bind<INavigationService>().To<DesktopNavigationService>().InSingletonScope();
    }

    void ConfigureVMVFactory(IViewModelViewFactory f)
    { }
  }
}