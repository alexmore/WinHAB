using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Desktop.ViewModels
{
  public class DesktopMainViewModel : MainViewModel
  {
    public DesktopMainViewModel(INavigationService navigationService, AppConfiguration appConfig, OpenHabClient client, IWidgetsFactory widgetsFactory, IEnumerable<SitemapData> sitemaps, SitemapData selectedSitemap) : 
      base(navigationService, appConfig, client,  widgetsFactory, sitemaps, selectedSitemap)
    {
    }

    public override void OnNavigatedTo()
    {
      base.OnNavigatedTo();

    }

    #region Appearance

    #endregion

  }
}