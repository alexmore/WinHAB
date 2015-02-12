using System.Collections.Generic;
using System.Collections.ObjectModel;
using WinHAB.Core.Collections;
using WinHAB.Core.Configuration;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class MainViewModel : ViewModel
  {
    private OpenHabClient _client;

    public MainViewModel(INavigationService navigationService, AppConfiguration appConfig, OpenHabClient client, IEnumerable<SitemapData> sitemaps, SitemapData selectedSitemap) : base(navigationService)
    {
      AppConfiguration = appConfig;

      Sitemap = selectedSitemap;
      _client = client;
      SitemapsList = sitemaps.ToObservableCollection();

      WidgetsListTitle = Sitemap.Label;
    }

    public AppConfiguration AppConfiguration { get; private set; }
    public INavigationService NavigationService { get { return Navigation; }}
    
    private string _widgetsListTitle;
    public string WidgetsListTitle { get { return _widgetsListTitle; } set { _widgetsListTitle = value; RaisePropertyChanged(() => WidgetsListTitle); } }

    private ObservableCollection<FrameWidget> _Widgets;
    public ObservableCollection<FrameWidget> Widgets { get { return _Widgets; } set { _Widgets = value; RaisePropertyChanged(() => Widgets); } }

    private SitemapData _Sitemap;
    public SitemapData Sitemap { get { return _Sitemap; } set { _Sitemap = value; RaisePropertyChanged(() => Sitemap); } }

    private ObservableCollection<SitemapData> _SitemapsList;
    public ObservableCollection<SitemapData> SitemapsList { get { return _SitemapsList; } set { _SitemapsList = value; RaisePropertyChanged(() => SitemapsList); } }

    public async override void OnNavigatedTo()
    {
      Waiter.Show();
      var page = await _client.GetPageAsync(Sitemap.HomepageLink);
      Widgets = new ObservableCollection<FrameWidget>();
      foreach (var i in page.Widgets)
      {
        var frame = new FrameWidget(Navigation) {Title = i.Label};
        frame.Widgets = new ObservableCollection<WidgetBase>();
        foreach (var w in i.Widgets)
        {
          frame.Widgets.Add(new WidgetBase(Navigation) { Size = WidgetSize.Meduim, Title = w.Label});
        }
        Widgets.Add(frame);
      }
      Waiter.Hide();
    }
  }
}