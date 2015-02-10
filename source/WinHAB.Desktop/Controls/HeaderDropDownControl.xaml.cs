using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Presentation;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Desktop.Windows;

namespace WinHAB.Desktop.Controls
{
  /// <summary>
  /// Interaction logic for HeaderDropDownControl.xaml
  /// </summary>
  public partial class HeaderDropDownControl : UserControl
  {
    public HeaderDropDownControl()
    {
      InitializeComponent();

      this.IsVisibleChanged += (sender, args) =>
      {
        DarkButton.IsChecked = AppearanceManager.Current.ThemeSource.OriginalString.ToLower() ==
                               AppearanceManager.DarkThemeSource.OriginalString.ToLower();
        LightButton.IsChecked = !DarkButton.IsChecked;
        AccentListBox.SelectedItem = AppearanceManager.Current.AccentColor;
      };
    }

    public static readonly DependencyProperty AppConfigurationProperty = DependencyProperty.Register(
      "AppConfiguration", typeof (AppConfiguration), typeof (HeaderDropDownControl), new PropertyMetadata(default(AppConfiguration)));

    public AppConfiguration AppConfiguration
    {
      get { return (AppConfiguration) GetValue(AppConfigurationProperty); }
      set { SetValue(AppConfigurationProperty, value); }
    }

    public static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.Register(
      "NavigationService", typeof (INavigationService), typeof (HeaderDropDownControl), new PropertyMetadata(default(INavigationService)));

    public INavigationService NavigationService
    {
      get { return (INavigationService) GetValue(NavigationServiceProperty); }
      set { SetValue(NavigationServiceProperty, value); }
    }

    private void SetDarkTheme(object sender, RoutedEventArgs e)
    {
      AppearanceManager.Current.ThemeSource = AppearanceManager.DarkThemeSource;
      if (AppConfiguration != null)
      {
        AppConfiguration.ThemeSource = AppearanceManager.DarkThemeSource.OriginalString;
        AppConfiguration.Save();
      }
    }

    private void SetLightTheme(object sender, RoutedEventArgs e)
    {
      AppearanceManager.Current.ThemeSource = AppearanceManager.LightThemeSource;
      if (AppConfiguration != null)
      {
        AppConfiguration.ThemeSource = AppearanceManager.LightThemeSource.OriginalString;
        AppConfiguration.Save();
      }
    }

    private void SetAccentColor(object sender, SelectionChangedEventArgs e)
    {
      if (AccentListBox.SelectedItem == null) return;
      AppearanceManager.Current.AccentColor = (Color) AccentListBox.SelectedItem;
      if (AppConfiguration != null)
      {
        AppConfiguration.AccentColor = AppearanceManager.Current.AccentColor.ToHexString();
        AppConfiguration.Save();
      }
    }

    private void ChangeServer(object sender, RoutedEventArgs e)
    {
      if (NavigationService != null)
      {
        NavigationService.ClearHistory();
        NavigationService.Navigate<LaunchViewModel>();
      }
    }

    private async void ChangeSitemap(object sender, RoutedEventArgs e)
    {
      if (NavigationService != null)
      {
        NavigationService.ClearHistory();
        var vm = NavigationService.Navigate<LaunchViewModel>();
        await vm.ConnectCommand.ExecuteAsync(AppConfiguration.Server);
      }
    }

    private void SetLanguage(object sender, SelectionChangedEventArgs e)
    {
      if (LanguageComboBox.SelectedItem == null || (LanguageComboBox.SelectedItem as WinHAB.Core.Configuration.Language) == null) return;

      var language = (LanguageComboBox.SelectedItem as Language);
      AppConfiguration.Language = language.Culture;
      AppConfiguration.Save();

      if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower() != AppConfiguration.Language.ToLower())
        RestartToApplyButton.Visibility = Visibility.Visible;
      else
        RestartToApplyButton.Visibility = Visibility.Collapsed;
    }

    private void RestartToApply(object sender, RoutedEventArgs e)
    {
      System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
      Application.Current.Shutdown();
    }
  }
}
