using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;
using WinHAB.Desktop.Localization;

namespace WinHAB.Desktop.ViewModels.Settings
{
  public class SettingsAppearanceViewModel : ViewModel, ITitledViewModel
  {
    private readonly DesktopConfiguration _appConfig;
    private readonly INavigationService _navigation;

    public SettingsAppearanceViewModel(INavigationService navigation, DesktopConfiguration appConfig)
    {
      _appConfig = appConfig;
      _navigation = navigation;
      Title = Strings.TitleAppearance;
      _SelectedAccentColor =
        _appConfig.AccentColors.FirstOrDefault(x => x.ToColor() == FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor);

      AccentColors = new ObservableCollection<string>(_appConfig.AccentColors);
      
      BackgroundImageSource = _appConfig.BackgroundImage;
      BrowseBackgroundCommand = new RelayCommand(BrowseBackground);
      SetBackgroundImageCommand = new RelayCommand<string>(SetBackgroundImage);
      RestartApplicationCommand = new RelayCommand(() =>
      {
        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
      });

      Languages = new ObservableCollection<Language>(_appConfig.AvailableLanguages);
      SelectedLanguageCulture = _appConfig.Language;
    }

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(()=>Title); }}

    private ObservableCollection<string> _AccentColors;
    public ObservableCollection<string> AccentColors { get { return _AccentColors; } set { _AccentColors = value; RaisePropertyChanged(()=>AccentColors); }}

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

      _appConfig.AccentColor = FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor.ToHexString();
      _appConfig.Save();
    }

    private string _BackgroundImageSource;
    public string BackgroundImageSource { get { return _BackgroundImageSource; } set { _BackgroundImageSource = value; RaisePropertyChanged(()=>BackgroundImageSource); }}

    public RelayCommand BrowseBackgroundCommand { get; set; }
    // TODO: replace OpenFileDialog with NavigationService
    private void BrowseBackground()
    {
      var d = new OpenFileDialog();
      d.DefaultExt = "*.jpg";
      d.Filter = Localization.Strings.LabelImageFiles + "|*.jpg";
      bool? dRes = d.ShowDialog();
      if (dRes.HasValue && dRes.Value)
      {
        SetBackgroundImage(d.FileName);
      }
    }

    public RelayCommand<string> SetBackgroundImageCommand { get; set; }

    private void SetBackgroundImage(string path)
    {
      try
      {
        _appConfig.SetBackground(path);
        BackgroundImageSource = _appConfig.BackgroundImage;
      }
      catch (Exception ex)
      {
        _navigation.ShowMessage(Localization.Strings.MessageExceptionOnApplyImageToBackgroundTitle,
          Localization.Strings.MessageExceptionOnApplyImageToBackground + "\r\n" + ex.Message, () => { });
      }
    }

    private ObservableCollection<Language> _Languages;
    public ObservableCollection<Language> Languages { get { return _Languages; } set { _Languages = value; RaisePropertyChanged(()=>Languages); }}

    public RelayCommand RestartApplicationCommand { get; set; }

    private bool _IsRestartRequired;
    public bool IsRestartRequired { get { return _IsRestartRequired; } set { _IsRestartRequired = value; RaisePropertyChanged(() => IsRestartRequired); } }

    private string _SelectedLanguageCulture;

    public string SelectedLanguageCulture
    {
      get { return _SelectedLanguageCulture; }
      set { _SelectedLanguageCulture = value; RaisePropertyChanged(() => SelectedLanguageCulture); SetLanguage(value); }
    }

    void SetLanguage(string culture)
    {
      if (culture == null) return;

      _appConfig.Language = culture;
      RaisePropertyChanged(() => _appConfig.Language);
      _appConfig.Save();

      IsRestartRequired = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower() != _appConfig.Language.ToLower();
    }
  }
}