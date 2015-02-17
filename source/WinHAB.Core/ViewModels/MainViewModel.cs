using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;
using WinHAB.Core.Models;
using WinHAB.Core.Mvvm;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class MainViewModel : ViewModel
  {
    protected readonly OpenHabClient OpenHabClient;
    private readonly IWidgetsFactory _widgetsFactory;

    public MainViewModel(INavigationService navigationService, 
      AppConfiguration appConfig, OpenHabClient openHabClient, IWidgetsFactory widgetsFactory, 
      SitemapData selectedSitemap) : base(navigationService)
    {
      AppConfiguration = appConfig;
      Sitemap = selectedSitemap;
      OpenHabClient = openHabClient;
      _widgetsFactory = widgetsFactory;

      Title = Sitemap.Label;

      LoadLinkedPageCommand = new AsyncRelayCommand<WidgetBase>(LoadLinkedPage);
      HistoryBackCommand = new AsyncRelayCommand(HistoryBack);
    }

    public AppConfiguration AppConfiguration { get; private set; }
    public INavigationService NavigationService { get { return Navigation; }}
    
    private string _title;
    public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(() => Title); } }

    private ObservableCollection<FrameWidget> _Widgets;
    public ObservableCollection<FrameWidget> Widgets { get { return _Widgets; } set { if (_Widgets != null) CleanupWidgets(); _Widgets = value; RaisePropertyChanged(() => Widgets); } }

    private SitemapData _Sitemap;
    public SitemapData Sitemap { get { return _Sitemap; } set { _Sitemap = value; RaisePropertyChanged(() => Sitemap); } }

    public async override void OnNavigatedTo()
    {
      await LoadPageWidgets(Sitemap.HomepageLink);

      _currentPage = new PagesHistoryItem() {Title = Sitemap.Label, Uri = Sitemap.HomepageLink};
      
      AppConfiguration.Sitemap = Sitemap.Name;
      await AppConfiguration.SaveAsync();
    }

    private async Task LoadPageWidgets(Uri pageUri)
    {
      const string fakeFrameLabel = "#WRAPPER#";

      Waiter.Show();
      
      var page = await OpenHabClient.GetPageAsync(pageUri);
      if (page == null || page.Widgets == null || page.Widgets.Count == 0) return;

      // Wrap independent widgets into frames
      var framesData = new List<WidgetData>();
      foreach (var widget in page.Widgets)
      {
        if (widget.Type == WidgetType.Frame) framesData.Add(widget);
        else
        {
          if (framesData.Count == 0 || framesData.Last() == null || framesData.Last().Label != fakeFrameLabel) 
            framesData.Add(new WidgetData() { Widgets = new List<WidgetData>(), Label = fakeFrameLabel, Type = WidgetType.Frame });
          framesData.Last().Widgets.Add(widget);
        }
      }

      var widgetsInitializators = new List<Task>();

      var res = new List<FrameWidget>();
      foreach (var frameData in framesData)
      {
        if (frameData.Label == fakeFrameLabel) frameData.Label = null;
        
        var frame = (FrameWidget)_widgetsFactory.Create(frameData);
        res.Add(frame);
        widgetsInitializators.Add(frame.Initialize());

        foreach (var widgetData in frameData.Widgets)
        {
          var widget = _widgetsFactory.Create(widgetData);
          if (widget != null)
          {
            frame.Widgets.Add(widget);
            widgetsInitializators.Add(widget.Initialize());
          }
        }
      }

      Widgets = res.ToObservableCollection();
      Waiter.Hide();

      Parallel.ForEach(widgetsInitializators, async x => await x);
    }

    private void CleanupWidgets()
    {
      if (Widgets != null)
        foreach (var frame in Widgets)
          frame.Cleanup();
    }

    #region History
    
    class PagesHistoryItem
    {
      public string Title { get; set; }
      public Uri Uri { get; set; }
    }

    readonly Stack<PagesHistoryItem> _pagesHistory = new Stack<PagesHistoryItem>();
    private PagesHistoryItem _currentPage;

    public bool HasHistory { get { return _pagesHistory.Count > 0; } }

    private string _HistoryPath;
    public string HistoryPath { get { return _HistoryPath; } set { _HistoryPath = value; RaisePropertyChanged(() => HistoryPath); } }
    
    public AsyncRelayCommand<WidgetBase> LoadLinkedPageCommand { get; set; }

    async Task LoadLinkedPage(WidgetBase widget)
    {
      if (widget.LinkedPageUri != null)
      {
        await LoadPageWidgets(widget.LinkedPageUri);
        Title = widget.Title + " " + widget.Value;

        if (_currentPage != null)
        {
          _pagesHistory.Push(_currentPage);
          RaisePropertyChanged(()=>HasHistory);
          HistoryPath = GetHistoryPath();
        }
        _currentPage = new PagesHistoryItem() {Title = Title, Uri = widget.LinkedPageUri};

      }
    }

    public AsyncRelayCommand HistoryBackCommand { get; set; }

    async Task HistoryBack()
    {
      if (_pagesHistory.Count == 0) return;
      _currentPage = _pagesHistory.Pop();

      if (_currentPage != null)
      {
        await LoadPageWidgets(_currentPage.Uri);
        Title = _currentPage.Title;
        RaisePropertyChanged(() => HasHistory);
        HistoryPath = GetHistoryPath();
      }
    }

    string GetHistoryPath()
    {
      var path = string.Join(" → ", _pagesHistory.ToArray().Reverse().Select(x => x.Title));
      return !string.IsNullOrWhiteSpace(path) ? path + " → " : path;
    }

    #endregion

  }
}