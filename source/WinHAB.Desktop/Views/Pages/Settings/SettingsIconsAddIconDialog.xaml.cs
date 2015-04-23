using System.Windows.Controls;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Desktop.ViewModels.Settings;

namespace WinHAB.Desktop.Views.Pages.Settings
{
  /// <summary>
  /// Interaction logic for SettingsIconsAddIconDialog.xaml
  /// </summary>
  [ViewModel(typeof(SettingsIconsAddIconDialogModel))]
  public partial class SettingsIconsAddIconDialog : UserControl, IView
  {
    public SettingsIconsAddIconDialog()
    {
      InitializeComponent();
    }
  }
}
