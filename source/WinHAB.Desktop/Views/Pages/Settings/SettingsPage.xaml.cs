using System.Windows.Controls;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Desktop.ViewModels.Settings;

namespace WinHAB.Desktop.Views.Pages.Settings
{
  /// <summary>
  /// Interaction logic for SettingsPage.xaml
  /// </summary>
  [ViewModel(typeof(SettingsPageModel))]
  public partial class SettingsPage : UserControl, IView
  {
    public SettingsPage()
    {
      InitializeComponent();
    }
  }
}
