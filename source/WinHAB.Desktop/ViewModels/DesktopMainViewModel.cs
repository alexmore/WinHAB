using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.ViewModels
{
  public class DesktopMainViewModel : MainViewModel
  {
    public DesktopMainViewModel(INavigationService navigationService, AppConfiguration appConfig, OpenHabClient client, IWidgetsFactory widgetsFactory, IEnumerable<SitemapData> sitemaps, SitemapData selectedSitemap) : 
      base(navigationService, appConfig, client,  widgetsFactory, sitemaps, selectedSitemap)
    {
    }

    public async override void OnNavigatedTo()
    {
      Waiter.Show();
      await UserResources.LoadUserResources(AppConfiguration.Server);
      Waiter.Hide();
      
      base.OnNavigatedTo();
    }

    #region Appearance

    #endregion

  }
}