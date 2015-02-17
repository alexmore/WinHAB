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

namespace WinHAB.Desktop.Controls
{
  /// <summary>
  /// Interaction logic for WaiterControl.xaml
  /// </summary>
  public partial class WaiterControl : UserControl
  {
    public WaiterControl()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty WaiterProperty = DependencyProperty.Register(
     "Waiter", typeof(WaiterViewModel), typeof(WaiterControl), new PropertyMetadata(default(WaiterViewModel)));

    public WaiterViewModel Waiter
    {
      get { return (WaiterViewModel)GetValue(WaiterProperty); }
      set { SetValue(WaiterProperty, value); }
    }
  }
}
