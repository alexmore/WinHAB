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
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Desktop.Views
{
  /// <summary>
  /// Interaction logic for LaunchView.xaml
  /// </summary>
  [ViewModel(typeof(BootstrapperPageModel))]
  public partial class LaunchView : IView
  {
    public LaunchView()
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
