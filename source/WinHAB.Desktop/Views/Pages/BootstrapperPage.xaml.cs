using System.Windows.Controls;
using System.Windows.Input;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Desktop.Views.Pages
{
  /// <summary>
  /// Interaction logic for LaunchView.xaml
  /// </summary>
  [ViewModel(typeof(BootstrapperPageModel))]
  public partial class BootstrapperPage : IView
  {
    public BootstrapperPage()
    {
      InitializeComponent();
    }

    private void ConnectToServer(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        var cmd = ConnectButton.Command;
        if (cmd.CanExecute((sender as TextBox).Text))
          cmd.Execute((sender as TextBox).Text);
      }
    }
  }
}
