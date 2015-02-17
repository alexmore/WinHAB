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
using WinHAB.Core.ViewModels;

namespace WinHAB.Desktop.Controls
{
  /// <summary>
  /// Interaction logic for TaskProgressControl.xaml
  /// </summary>
  public partial class TaskProgressControl : UserControl
  {
    public TaskProgressControl()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty TaskProgressProperty = DependencyProperty.Register(
     "TaskProgress", typeof(TaskProgressViewModel), typeof(TaskProgressControl), new PropertyMetadata(default(TaskProgressViewModel)));

    public TaskProgressViewModel TaskProgress
    {
      get { return (TaskProgressViewModel)GetValue(TaskProgressProperty); }
      set { SetValue(TaskProgressProperty, value); }
    }
  }
}
