using System.Windows.Controls;
using System.Windows.Input;

namespace WinHAB.Desktop.Views.Pages.Settings
{
  /// <summary>
  /// Interaction logic for SettingsIconsView.xaml
  /// </summary>
  public partial class SettingsIconsView : UserControl
  {
    public SettingsIconsView()
    {
      InitializeComponent();
    }

    private void HorizontalScroll(object sender, MouseWheelEventArgs e)
    {
      var scrollViewer = (sender as ScrollViewer);
      if (scrollViewer == null) return;

      scrollViewer.ScrollToHorizontalOffset(scrollViewer.ContentHorizontalOffset + e.Delta * -1);
    }
  }
}
