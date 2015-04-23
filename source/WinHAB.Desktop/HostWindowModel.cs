using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Desktop.Assets;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;
using WinHAB.Desktop.ViewModels;
using WinHAB.Desktop.ViewModels.Settings;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;

namespace WinHAB.Desktop
{
  public class HostWindowModel : PageModelBase
  {
    private readonly AppConfiguration _appConfig;
    private readonly IRestClientFactory _clientFactory;

    public HostWindowModel(INavigationService navigationService, AppConfiguration appConfig,
      IRestClientFactory clientFactory) : base(navigationService)
    {
      _appConfig = appConfig;
      _clientFactory = clientFactory;
      
      SettingsCommand = new RelayCommand(async () =>
      {
        IsPopupOpened = false;
        await Navigation.NavigateAsync<SettingsPageModel>();
      });

      SettingsConectionCommand = new RelayCommand(async () =>
      {
        IsPopupOpened = false;
        await Navigation.NavigateAsync<SettingsPageModel>(typeof(SettingsConnectionViewModel));
      });

      SettingsAppearanceCommand = new RelayCommand(async () =>
      {
        IsPopupOpened = false;
        await Navigation.NavigateAsync<SettingsPageModel>(typeof(SettingsAppearanceViewModel));
      });

      SettingsIconsCommand = new RelayCommand(async () =>
      {
        IsPopupOpened = false;
        await Navigation.NavigateAsync<SettingsPageModel>(typeof(SettingsIconsViewModel));
      });

      HelpCommand = new RelayCommand(() => { });
      AboutCommand = new RelayCommand(() => { });

      ExitCommand = new RelayCommand(Application.Current.Shutdown);

    }

    public override async Task InitializeAsync(object parameter)
    {
      ShowProgressIndicator();
      await UserResources.LoadUserResources(_appConfig.Server, _clientFactory);
      HideProgressIndicator();
    }

    private bool _IsPopupOpened;
    public bool IsPopupOpened { get { return _IsPopupOpened; } set { _IsPopupOpened = value; RaisePropertyChanged(()=>IsPopupOpened); }}

    public RelayCommand SettingsCommand { get; set; }
    public RelayCommand SettingsConectionCommand { get; set; }
    public RelayCommand SettingsAppearanceCommand { get; set; }
    public RelayCommand SettingsIconsCommand { get; set; }

    public RelayCommand AboutCommand { get; set; }
    public RelayCommand HelpCommand { get; set; }

    public RelayCommand ExitCommand { get; set; }

  }
}