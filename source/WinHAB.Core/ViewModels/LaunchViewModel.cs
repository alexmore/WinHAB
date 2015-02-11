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
      SelectSitemapCommand = new RelayCommand<SitemapData>(SelectSitemap);
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
    private bool _isServerAddressVisible = false;
    public bool IsServerAddressVisible { get { return _isServerAddressVisible; } set { _isServerAddressVisible = value; RaisePropertyChanged(() => IsServerAddressVisible); } }

    private bool _IsSitemapsVisible = false;
    public bool IsSitemapsVisible { get { return _IsSitemapsVisible; } set { _IsSitemapsVisible = value; RaisePropertyChanged(() => IsSitemapsVisible); } }

    void HideAll()
    {
      IsServerAddressVisible = false;
      IsSitemapsVisible = false;
    }

    void ShowServerUrl()
    {
      Waiter.Hide();
      IsServerAddressVisible = true;
      IsSitemapsVisible = false;
    }

    void ShowSitemaps()
    {
      Waiter.Hide();
      IsServerAddressVisible = false;
      IsSitemapsVisible = true;
    }
    #endregion

    public override void OnNavigatedTo()
    {
      ServerAddress = _config.Server;

      if (string.IsNullOrWhiteSpace(ServerAddress)) ServerAddress = "http://";
      ShowServerUrl();
    }

    async Task Connect(string server)
    {
      try
      {
        HideAll();
        Waiter.Show(Localization.Connecting);
        Sitemaps = new ObservableCollection<SitemapData>(await _client.GetSitemapsAsync(new Uri(server+"/rest/sitemaps/")));
        _config.Server = server;
        await _config.SaveAsync();
        ShowSitemaps();
      }
      catch (Exception e)
      {
        Navigation.ShowMessage(Localization.LaunchViewModelConnectionErrorTitle, Localization.LaunchViewModelConnectionError+" "+e.Message, ShowServerUrl);
      }
    }

    void SelectSitemap(SitemapData sitemap)
    {
      if (sitemap == null || sitemap.HomepageLink == null)
      {
        Navigation.ShowMessage(Localization.LaunchViewSelesiteMapHomepageLinkExceptonTitle, Localization.LaunchViewSelesiteMapHomepageLinkExcepton,
          () => { });
        return;
      }

      Navigation.Navigate<MainViewModel>(x => x.Add("homepageUri", sitemap.HomepageLink));
    }
  }
}