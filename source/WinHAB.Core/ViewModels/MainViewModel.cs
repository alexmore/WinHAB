using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly IWidgetsFactory _widgetsFactory;

    public MainViewModel(INavigationService navigationService, AppConfiguration appConfig, OpenHabClient client, IWidgetsFactory widgetsFactory, IEnumerable<SitemapData> sitemaps, SitemapData selectedSitemap) : base(navigationService)
    {
      AppConfiguration = appConfig;

      Sitemap = selectedSitemap;
      _client = client;
      _widgetsFactory = widgetsFactory;
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
      Widgets = await LoadPageWidgets(Sitemap.HomepageLink).ToObservableCollectionAsync();
    }

    private async Task<IEnumerable<FrameWidget>> LoadPageWidgets(Uri pageUri)
    {
      Waiter.Show();
      
      var page = await _client.GetPageAsync(pageUri);
      if (page == null || page.Widgets == null || page.Widgets.Count == 0) return null;

      // Wrap independent widgets into frames
      var framesData = new List<WidgetData>();
      foreach (var widget in page.Widgets)
      {
        if (widget.Type == WidgetType.Frame) framesData.Add(widget);
        else
        {
          if (framesData.Last() == null || framesData.Last().Label != "#WRAPPER#") framesData.Add(new WidgetData() { Widgets = new List<WidgetData>(), Label = "#WRAPPER#"});
          framesData.Last().Widgets.Add(widget);
        }
      }

      var res = new List<FrameWidget>();
      foreach (var frameData in framesData)
      {
        var frame = (FrameWidget)_widgetsFactory.Create(frameData);
        res.Add(frame);

        foreach (var widgetData in frameData.Widgets)
        {
          var widget = _widgetsFactory.Create(widgetData);
          if (widget != null) frame.Widgets.Add(widget);
        }
      }

      Waiter.Hide();

      return res;
    }

    #region History
    #endregion

  }
}