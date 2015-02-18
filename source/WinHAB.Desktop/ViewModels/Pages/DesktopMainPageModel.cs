using System.Collections.Generic;
using System.Threading.Tasks;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Desktop.Assets;

namespace WinHAB.Desktop.ViewModels.Pages
{
  public class DesktopMainPageModel : MainPageModel
  {
    public DesktopMainPageModel(INavigationService navigationService, AppConfiguration appConfig, IRestClientFactory clientFactory, WidgetsFactory widgetsFactory, IEnumerable<Sitemap> sitemaps, Sitemap selectedSitemap) : 
      base(navigationService, appConfig, clientFactory,  widgetsFactory)
    {
    }

    public async override Task InitializeAsync(dynamic parameter)
    {
      TaskProgress.Show();
      await UserResources.LoadUserResources(AppConfiguration.Server, ClientFactory);
      TaskProgress.Hide();

      await base.InitializeAsync((object)parameter);
    }

    #region Appearance

    #endregion

  }
}