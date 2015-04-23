using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace WinHAB.Desktop
{
  /// <summary>
  /// Interaction logic for HostWindow.xaml
  /// </summary>
  public partial class HostWindow : INavigationHost
  {
    public static readonly DependencyProperty CurrentTimeProperty = DependencyProperty.Register(
      "CurrentTime", typeof (string), typeof (HostWindow), new PropertyMetadata(default(string)));

    public string CurrentTime
    {
      get { return (string) GetValue(CurrentTimeProperty); }
      set { SetValue(CurrentTimeProperty, value); }
    }

    public static readonly DependencyProperty CurrentDateProperty = DependencyProperty.Register(
      "CurrentDate", typeof (string), typeof (HostWindow), new PropertyMetadata(default(string)));

    public string CurrentDate
    {
      get { return (string) GetValue(CurrentDateProperty); }
      set { SetValue(CurrentDateProperty, value); }
    }

    #region Fixes Popup placement issue on Tablet PC or desktop touch screens
    private static readonly FieldInfo _menuDropAlignmentField;
    static HostWindow()
    {
        _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
        System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);

        EnsureStandardPopupAlignment();
        SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
    }

    private static void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        EnsureStandardPopupAlignment();
    }

    private static void EnsureStandardPopupAlignment()
    {
        if (SystemParameters.MenuDropAlignment && _menuDropAlignmentField != null)
        {
            _menuDropAlignmentField.SetValue(null, false);
        }
    }
    #endregion

    public HostWindow()
    {
      InitializeComponent();

      Loaded += HostWindow_Loaded;
    }

    void HostWindow_Loaded(object sender, RoutedEventArgs e)
    {
      Action setCurrentDateTime = () =>
      {
        CurrentTime = DateTime.Now.ToShortTimeString();
        CurrentDate = DateTime.Now.ToString("dddd, dd MMMM yyyy");
      };

      setCurrentDateTime();

      var timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0, 0, 0, 1);
      timer.Tick += (s, args) => setCurrentDateTime();
      timer.Start();
    }
  }
}
