using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels.Pages
{
  public class MainPage : PageBase
  {
    protected readonly OpenHabClient OpenHabClient;
    private readonly WidgetsFactory _widgetsFactory;

    public MainPage(INavigationService navigationService, 
      AppConfiguration appConfig, OpenHabClient openHabClient, WidgetsFactory widgetsFactory) : base(navigationService)
    {
      AppConfiguration = appConfig;
      OpenHabClient = openHabClient;
      _widgetsFactory = widgetsFactory;
      
      LoadLinkedPageCommand = new AsyncRelayCommand<WidgetBase>(LoadLinkedPage);
      HistoryBackCommand = new AsyncRelayCommand(HistoryBack);
    }

    public AppConfiguration AppConfiguration { get; private set; }
    public INavigationService NavigationService { get { return Navigation; }}
    
    private string _title;
    public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(() => Title); } }

    private ObservableCollection<FrameWidget> _Widgets;
    public ObservableCollection<FrameWidget> Widgets { get { return _Widgets; } set { if (_Widgets != null) CleanupWidgets(); _Widgets = value; RaisePropertyChanged(() => Widgets); } }
    
    public override async Task InitializeAsync(dynamic parameter)
    {
      Title = parameter.Label;

      await LoadPageWidgets(parameter.HomepageLink);

      _currentPage = new PagesHistoryItem() { Title = parameter.Label, Uri = parameter.HomepageLink };

      AppConfiguration.Sitemap = parameter.Name;
      await AppConfiguration.SaveAsync();
    }

    private async Task LoadPageWidgets(Uri pageUri)
    {
      const string fakeFrameLabel = "#WRAPPER#";

      TaskProgress.Show();
      
      var page = await OpenHabClient.GetPageAsync(pageUri);
      if (page == null || page.Widgets == null || page.Widgets.Count == 0) return;

      // Wrap independent widgets into frames
      var framesData = new List<Widget>();
      foreach (var widget in page.Widgets)
      {
        if (widget.Type == WidgetType.Frame) framesData.Add(widget);
        else
        {
          if (framesData.Count == 0 || framesData.Last() == null || framesData.Last().Label != fakeFrameLabel) 
            framesData.Add(new Widget() { Widgets = new List<Widget>(), Label = fakeFrameLabel, Type = WidgetType.Frame });
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
      TaskProgress.Hide();

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
      if (widget.LinkedPage != null)
      {
        await LoadPageWidgets(widget.LinkedPage);
        Title = widget.Title + " " + widget.Value;

        if (_currentPage != null)
        {
          _pagesHistory.Push(_currentPage);
          RaisePropertyChanged(()=>HasHistory);
          HistoryPath = GetHistoryPath();
        }
        _currentPage = new PagesHistoryItem() {Title = Title, Uri = widget.LinkedPage};

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