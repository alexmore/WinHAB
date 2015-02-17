using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Localization;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels
{
  public class LaunchViewModel : PageBase
  {
    private OpenHabClient _client;
    private AppConfiguration _config;

    public LaunchViewModel(INavigationService navigationService, OpenHabClient client, AppConfiguration config) : base(navigationService)
    {
      _client = client;
      _config = config;

      ConnectCommand = new AsyncRelayCommand<string>(Connect, (string x) => 
        !string.IsNullOrWhiteSpace(ServerAddress));
      SelectSitemapCommand = new AsyncRelayCommand<Sitemap>(SelectSitemap);
    }

    public AsyncRelayCommand<string> ConnectCommand { get; set; }
    public AsyncRelayCommand<Sitemap> SelectSitemapCommand { get; set; }

    private string _ServerAddress;
    public string ServerAddress
    {
      get
      {
        return _ServerAddress;
      }
      set
      {
        _ServerAddress = value;
        RaisePropertyChanged(() => ServerAddress);
        ConnectCommand.RaiseCanExecuteChanged();
      }
    }

    private ObservableCollection<Sitemap> _Sitemaps;
    public ObservableCollection<Sitemap> Sitemaps { get { return _Sitemaps; } set { _Sitemaps = value; RaisePropertyChanged(() => Sitemaps); } }

    #region Appearance
    private bool _isServerAddressVisible = false;
    public bool IsServerAddressVisible { get { return _isServerAddressVisible; } set { _isServerAddressVisible = value; RaisePropertyChanged(() => IsServerAddressVisible); } }

    private bool _IsSitemapsVisible = false;
    public bool IsSitemapsVisible { get { return _IsSitemapsVisible; } set { _IsSitemapsVisible = value; RaisePropertyChanged(() => IsSitemapsVisible); } }

    public void HideAll()
    {
      IsServerAddressVisible = false;
      IsSitemapsVisible = false;
    }

    public void ShowServerUrl()
    {
      TaskProgress.Hide();
      IsServerAddressVisible = true;
      IsSitemapsVisible = false;
    }

    public void ShowSitemaps()
    {
      TaskProgress.Hide();
      IsServerAddressVisible = false;
      IsSitemapsVisible = true;
    }
    #endregion

    public override Task InitializeAsync(dynamic parameter)
    {
      ServerAddress = _config.Server;

      if (string.IsNullOrWhiteSpace(ServerAddress)) ServerAddress = "http://";
      ShowServerUrl();

      return Task.FromResult(0);
    }

    async Task Connect(string server)
    {
      try
      {
        HideAll();
        TaskProgress.Show(Strings.TaskConnecting);
        Sitemaps = new ObservableCollection<Sitemap>(await _client.GetSitemapsAsync(new Uri(server+"/rest/sitemaps/")));
        bool showSitemaps = _config.Server != server;
        _config.Server = server;
        await _config.SaveAsync();

        var existingSitemap = Sitemaps.FirstOrDefault(x => x.Name == _config.Sitemap);
        if (existingSitemap == null || showSitemaps) 
          ShowSitemaps();
        else
          SelectSitemapCommand.Execute(existingSitemap);
      }
      catch (Exception e)
      {
        Navigation.ShowMessage(Strings.MessageConnectionErrorTitle, Strings.MessageConnectionError + " " + e.Message, ShowServerUrl);
      }
    }

    async Task SelectSitemap(Sitemap sitemap)
    {
      if (sitemap == null || sitemap.HomepageLink == null)
      {
        Navigation.ShowMessage(Strings.MessageHomepageLinkMissedInSitemapTitle, Strings.MessageHomepageLinkMissedInSitemap,
          () => { });
        return;
      }

      await Navigation.NavigateAsync<MainViewModel>(sitemap);
    }
  }
}