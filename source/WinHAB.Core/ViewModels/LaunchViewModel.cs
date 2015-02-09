using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Configuration;
using WinHAB.Core.Localizations;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels
{
  public class LaunchViewModel : ViewModel
  {
    private OpenHabClient _client;
    private AppConfiguration _config;

    public LaunchViewModel(INavigationService navigationService, OpenHabClient client, AppConfiguration config) : base(navigationService)
    {
      _client = client;
      _config = config;

      ConnectCommand = new AsyncRelayCommand<string>(Connect, (string x) => 
        !string.IsNullOrWhiteSpace(ServerAddress));
    }

    public AsyncRelayCommand<string> ConnectCommand { get; set; }
    public RelayCommand<SitemapData> SelectSitemapCommand { get; set; }

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

    private ObservableCollection<SitemapData> _Sitemaps;
    public ObservableCollection<SitemapData> Sitemaps { get { return _Sitemaps; } set { _Sitemaps = value; RaisePropertyChanged(() => Sitemaps); } }

    #region Appearance
    private bool _IsServerUrlVisible;
    public bool IsServerUrlVisible { get { return _IsServerUrlVisible; } set { _IsServerUrlVisible = value; RaisePropertyChanged(() => IsServerUrlVisible); } }

    private bool _IsSitemapsVisible;
    public bool IsSitemapsVisible { get { return _IsSitemapsVisible; } set { _IsSitemapsVisible = value; RaisePropertyChanged(() => IsSitemapsVisible); } }

    void ShowServerUrl()
    {
      Waiter.Hide();
      IsServerUrlVisible = true;
      IsSitemapsVisible = false;
    }

    void ShowSitemaps()
    {
      Waiter.Hide();
      IsServerUrlVisible = false;
      IsSitemapsVisible = true;
    }
    #endregion

    public override void OnNavigatedTo()
    {
      ServerAddress = _config.Server;

      if (string.IsNullOrWhiteSpace(ServerAddress)) ServerAddress = "http://";
    }

    async Task Connect(string server)
    {
      try
      {
        Waiter.Show(Localization.Connecting);
        _client.SetServerAddress(server);
        Sitemaps = new ObservableCollection<SitemapData>(await _client.GetSitemapsAsync());
        _config.Server = server;
        await _config.SaveAsync();
        ShowSitemaps();
      }
      catch (Exception e)
      {
        Navigation.ShowMessage(Localization.aunchViewModelConnectionErrorTitle, Localization.LaunchViewModelConnectionError+" "+e.Message, ShowServerUrl);
      }
    }

    void SelectSitemap(SitemapData sitemap)
    {
      if (sitemap == null || sitemap.HomepageLink == null) return;

      Navigation.Navigate<MainViewModel>(x => x.Add("homepageUri", sitemap.HomepageLink));
    }
  }
}