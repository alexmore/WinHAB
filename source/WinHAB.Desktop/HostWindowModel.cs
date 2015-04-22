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
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;

namespace WinHAB.Desktop
{
  public class HostWindowModel : PageModelBase
  {
    private readonly IRestClientFactory _clientFactory;

    public HostWindowModel(INavigationService navigationService, AppConfiguration appConfig,
      IRestClientFactory clientFactory) : base(navigationService)
    {
      _clientFactory = clientFactory;
      AppConfig = appConfig as DesktopConfiguration;

      _SelectedAccentColor =
        AppConfig.AccentColors.FirstOrDefault(x => x.ToColor() == FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor);
      
      ChangeServerCommand = new AsyncRelayCommand(async () =>
      {
        IsPopupOpened = false;
        Navigation.ClearHistory();
        AppConfig.Runtime.IsRestarting = true;
        await Navigation.NavigateAsync<BootstrapperPageModel>();
      });

      ChangeSitemapCommand = new AsyncRelayCommand(async () =>
      {
        IsPopupOpened = false;
        AppConfig.Sitemap = null;
        Navigation.ClearHistory();
        await Navigation.NavigateAsync<BootstrapperPageModel>();
      });

      RestartApplicationCommand = new RelayCommand(() =>
      {
        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
      });
      
      SelectBackgroundCommand = new RelayCommand(SelectBackground);
      
    }

    public override async Task InitializeAsync(object parameter)
    {
      ShowProgressIndicator();
      await UserResources.LoadUserResources(AppConfig.Server, _clientFactory);
      HideProgressIndicator();

      SelectedLanguageCulture = AppConfig.Language;

      await base.InitializeAsync((object) parameter);
    }

    private bool _IsPopupOpened;
    public bool IsPopupOpened { get { return _IsPopupOpened; } set { _IsPopupOpened = value; RaisePropertyChanged(()=>IsPopupOpened); }}

    public DesktopConfiguration AppConfig { get; private set; }

    #region Settings

    public AsyncRelayCommand ChangeServerCommand { get; set; }
    public AsyncRelayCommand ChangeSitemapCommand { get; set; }

    #endregion
    
    #region Language

    public RelayCommand RestartApplicationCommand { get; set; }

    private bool _IsRestartRequired;
    public bool IsRestartRequired { get { return _IsRestartRequired; } set { _IsRestartRequired = value; RaisePropertyChanged(() => IsRestartRequired); } }

    private string _SelectedLanguageCulture;

    public string SelectedLanguageCulture
    {
      get { return _SelectedLanguageCulture; } 
      set { _SelectedLanguageCulture = value; RaisePropertyChanged(() => SelectedLanguageCulture); SetLanguage(value);}
    }
    
    void SetLanguage(string culture)
    {
      if (culture == null) return;

      AppConfig.Language = culture;
      RaisePropertyChanged(() => AppConfig.Language);
      AppConfig.Save();

      IsRestartRequired = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower() != AppConfig.Language.ToLower();
    }

    #endregion

    #region Background
    
    public RelayCommand SelectBackgroundCommand { get; set; }

    private void SelectBackground()
    {
      var d = new OpenFileDialog();
      d.DefaultExt = "*.jpg";
      d.Filter = Localization.Strings.LabelImageFiles + "|*.jpg";
      bool? dRes = d.ShowDialog();
      if (dRes.HasValue && dRes.Value)
      {
        try
        {
          AppConfig.SetBackground(d.FileName);
        }
        catch (Exception ex)
        {
          Navigation.ShowMessage(Localization.Strings.MessageExceptionOnApplyImageToBackgroundTitle,
            Localization.Strings.MessageExceptionOnApplyImageToBackground + "\r\n" + ex.Message, () => { });
        }
      }
    }
    #endregion

    #region Accent color

    private string _SelectedAccentColor;
    public string SelectedAccentColor
    {
      get { return _SelectedAccentColor; }
      set
      {
        _SelectedAccentColor = value;
        RaisePropertyChanged(() => SelectedAccentColor);
        SetAccentColor(value.ToColor());
      }
    }

    void SetAccentColor(Color color)
    {
      FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor = color;
      
      AppConfig.AccentColor = FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor.ToHexString();
      AppConfig.Save();
    }
    #endregion

  }
}