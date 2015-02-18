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
using Microsoft.Win32;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;

namespace WinHAB.Desktop.Controls
{
  /// <summary>
  /// Interaction logic for HeaderDropDownControl.xaml
  /// </summary>
  public partial class SettingsDropDownControl : UserControl
  {
    public SettingsDropDownControl()
    {
      InitializeComponent();
    }
  }
}
