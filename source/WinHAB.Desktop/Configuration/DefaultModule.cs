using System.IO;
using Ninject.Modules;
using WinHAB.Core.Configuration;

namespace WinHAB.Desktop.Configuration
{
  public class DefaultModule : NinjectModule
  {
    public override void Load()
    {
      if (!Directory.Exists(AppConstants.ConfigurationFolder))
        Directory.CreateDirectory(AppConstants.ConfigurationFolder);

      Bind<string>().ToConstant(AppConstants.ConfigurationFile).WhenInjectedInto<JsonConfigurationProvider>();
      Bind<IConfigurationProvider>().To<JsonConfigurationProvider>();
    }
  }
}