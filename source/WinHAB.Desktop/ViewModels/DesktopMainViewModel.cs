using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.ViewModels
{
  public class DesktopMainViewModel : MainViewModel
  {
    public DesktopMainViewModel(INavigationService navigationService, AppConfiguration appConfig, OpenHabClient openHabClient, WidgetsFactory widgetsFactory, IEnumerable<Sitemap> sitemaps, Sitemap selectedSitemap) : 
      base(navigationService, appConfig, openHabClient,  widgetsFactory)
    {
    }

    public async override Task InitializeAsync(dynamic parameter)
    {
      TaskProgress.Show();
      await UserResources.LoadUserResources(AppConfiguration.Server, OpenHabClient);
      TaskProgress.Hide();

      await base.InitializeAsync((object)parameter);
    }

    #region Appearance

    #endregion

  }
}