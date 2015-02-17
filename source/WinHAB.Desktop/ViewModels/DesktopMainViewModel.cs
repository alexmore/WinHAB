using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Models;
using WinHAB.Core.Mvvm;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.ViewModels
{
  public class DesktopMainViewModel : MainViewModel
  {
    public DesktopMainViewModel(INavigationService navigationService, AppConfiguration appConfig, OpenHabClient openHabClient, IWidgetsFactory widgetsFactory, IEnumerable<SitemapData> sitemaps, SitemapData selectedSitemap) : 
      base(navigationService, appConfig, openHabClient,  widgetsFactory, selectedSitemap)
    {
    }

    public async override void OnNavigatedTo()
    {
      Waiter.Show();
      await UserResources.LoadUserResources(AppConfiguration.Server, OpenHabClient);
      Waiter.Hide();
      
      base.OnNavigatedTo();
    }

    #region Appearance

    #endregion

  }
}