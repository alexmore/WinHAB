﻿using System;
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
  public class MainPageModel : PageModelBase
  {
    protected readonly IRestClientFactory ClientFactory;
    private readonly IWidgetsFactory _widgetsFactory;

    public MainPageModel(INavigationService navigationService, IRestClientFactory clientFactory, IWidgetsFactory widgetsFactory) : base(navigationService)
    {
      ClientFactory = clientFactory;
      _widgetsFactory = widgetsFactory;
      
      LoadLinkedPageCommand = new AsyncRelayCommand<WidgetModelBase>(LoadLinkedPage);
      HistoryBackCommand = new AsyncRelayCommand(HistoryBack);
    }

    private string _title;
    public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(() => Title); } }

    private ObservableCollection<FrameWidgetModel> _Widgets;
    public ObservableCollection<FrameWidgetModel> Widgets { get { return _Widgets; } set { if (_Widgets != null) CleanupWidgets(); _Widgets = value; RaisePropertyChanged(() => Widgets); } }

    public override async Task InitializeAsync(object parameter)
    {
      var sitemap = parameter as Sitemap;
      if (sitemap == null)
        throw new ArgumentException("parameter must be of Sitemap type at MainPageModel.InitializeAsync.");

      Title = sitemap.Label;

      await LoadPageWidgets(sitemap.HomepageLink);

      _currentPage = new PagesHistoryItem() { Title = sitemap.Label, Uri = sitemap.HomepageLink };
    }

    private async Task LoadPageWidgets(Uri pageUri)
    {
      ShowProgressIndicator();

      Page page = null;

      using (var cln = ClientFactory.Create())
      {
        page = await cln.GetAsync(pageUri).AsPageAsync();
      }
      
      if (page == null || page.Widgets == null || page.Widgets.Count == 0) return;
      
      var widgetsInitializators = new List<Task>();
      var res = new List<FrameWidgetModel>();
      
      foreach (var frameData in page.Widgets.CreateFrameWrappedCollection())
      {
        var frame = (FrameWidgetModel)_widgetsFactory.Create(frameData);
        res.Add(frame);
        widgetsInitializators.Add(frame.InitializeAsync(null));

        foreach (var widget in frameData.Widgets.Select(widgetData => _widgetsFactory.Create(widgetData)).Where(widget => widget != null))
        {
          if (widget is INavigationWidget)
            (widget as INavigationWidget).NavigateLinkedPageCommand = LoadLinkedPageCommand;
          frame.Widgets.Add(widget);
          widgetsInitializators.Add(widget.InitializeAsync(null));
        }
      }

      Widgets = res.ToObservableCollection();
      HideProgressIndicator();

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
    
    public AsyncRelayCommand<WidgetModelBase> LoadLinkedPageCommand { get; set; }

    async Task LoadLinkedPage(WidgetModelBase widget)
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