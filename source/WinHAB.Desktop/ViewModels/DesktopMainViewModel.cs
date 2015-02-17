using System.Collections.Generic;
using System.Collections.ObjectModel;
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
      base(navigationService, appConfig, openHabClient,  widgetsFactory, selectedSitemap)
    {
    }

    public async override void OnLoaded()
    {
      TaskProgress.Show();
      await UserResources.LoadUserResources(AppConfiguration.Server, OpenHabClient);
      TaskProgress.Hide();
      
      base.OnLoaded();
    }

    #region Appearance

    #endregion

  }
}