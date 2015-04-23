using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Localization;

namespace WinHAB.Desktop.ViewModels.Settings
{
  public class SettingsConnectionViewModel : ViewModel, ITitledViewModel
  {
    private readonly INavigationService _navigation;
    private readonly AppConfiguration _config;

    public SettingsConnectionViewModel(INavigationService navigation, DesktopConfiguration config)
    {
      _navigation = navigation;
      _config = config;
      Title = Strings.TitleConnectionView;

      ChangeServerCommand = new AsyncRelayCommand(async () =>
      {
        _navigation.ClearHistory();
        _config.Runtime.IsRestarting = true;
        await _navigation.NavigateAsync<BootstrapperPageModel>();
      });

      ChangeSitemapCommand = new AsyncRelayCommand(async () =>
      {
        _config.Sitemap = null;
        _navigation.ClearHistory();
        await _navigation.NavigateAsync<BootstrapperPageModel>();
      });

      Server = _config.Server;
      Sitemap = _config.Sitemap;
    }

    public AsyncRelayCommand ChangeServerCommand { get; set; }
    public AsyncRelayCommand ChangeSitemapCommand { get; set; }
    
    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(()=>Title); }}

    private string _Server;
    public string Server { get { return _Server; } set { _Server = value; RaisePropertyChanged(()=>Server); }}

    private string _Sitemap;
    public string Sitemap { get { return _Sitemap; } set { _Sitemap = value; RaisePropertyChanged(()=>Sitemap); }}
  }
}