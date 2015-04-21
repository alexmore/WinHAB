using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Localization;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels.Pages
{
  public class BootstrapperPageModel : PageModelBase
  {
    private IRestClientFactory _clientFactory;
    private AppConfiguration _config;

    public BootstrapperPageModel(INavigationService navigationService, IRestClientFactory clientFactory, AppConfiguration config) : base(navigationService)
    {
      _clientFactory = clientFactory;
      _config = config;
      
      ConnectCommand = new AsyncRelayCommand<string>(Connect, (string x) => 
        !string.IsNullOrWhiteSpace(ServerAddress));
      SelectSitemapCommand = new AsyncRelayCommand<Sitemap>(SelectSitemap);

      ServerAddress = _config.Server;
    }

    public AsyncRelayCommand<string> ConnectCommand { get; set; }
    public AsyncRelayCommand<Sitemap> SelectSitemapCommand { get; set; }

    public override async Task InitializeAsync(object parameter)
    {
      ShowTaskProgress(Strings.TaskStarting);
      
      if (string.IsNullOrWhiteSpace(ServerAddress) || _config.Runtime.IsRestarting)
      {
        _config.Runtime.IsRestarting = false;
        ServerAddress = string.IsNullOrWhiteSpace(ServerAddress) ? "http://" : ServerAddress;
        ShowServerUrl();
      }
      else
        await ConnectCommand.ExecuteAsync(ServerAddress);
    }

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

    private void HideAll()
    {
      HideProgressIndicator();
      IsServerAddressVisible = false;
      IsSitemapsVisible = false;
    }

    private void ShowTaskProgress(string text)
    {
      HideAll();
      ShowProgressIndicator();
    }

    public void ShowServerUrl()
    {
      HideAll();
      IsServerAddressVisible = true;
    }

    public void ShowSitemaps()
    {
      HideAll();
      IsSitemapsVisible = true;
    }
    #endregion

    async Task Connect(string server)
    {
      try
      {
        ShowTaskProgress(Strings.TaskConnecting);

        using (var cln = _clientFactory.Create())
        {
          Sitemaps =
            new ObservableCollection<Sitemap>(await cln.GetAsync(new Uri(server + "/rest/sitemaps/")).AsSitemapListAsync());
        }
        bool showSitemaps = false;
        if (_config.Server != server)
        {
          showSitemaps = true;
          _config.Server = server;
          await _config.SaveAsync();
        }

        var savedSitemap = Sitemaps.FirstOrDefault(x => x.Name == _config.Sitemap);
        if (savedSitemap == null || showSitemaps) 
          ShowSitemaps();
        else
          SelectSitemapCommand.Execute(savedSitemap);
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

      try
      {
        await Navigation.NavigateAsync<MainPageModel>(sitemap);
      }
      catch (Exception e)
      {
        Navigation.ShowMessage(Strings.MessageMainPageModelNavigationFailedTitle, Strings.MessageMainPageModelNavigationFailed + "\r\n"+e.Message,
          () => { });
        
        ShowSitemaps();
        
        return;
      }
      
      _config.Sitemap = sitemap.Name;
      await _config.SaveAsync();
    }
  }
}