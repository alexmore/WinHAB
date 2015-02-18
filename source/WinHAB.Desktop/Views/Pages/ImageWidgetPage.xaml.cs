using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Desktop.Views.Pages
{
  /// <summary>
  /// Interaction logic for ImageWidgetPageView.xaml
  /// </summary>
  [ViewModel(typeof(ImageWidgetPageModel))]
  public partial class ImageWidgetPage : IView
  {
    public ImageWidgetPage()
    {
      InitializeComponent();
    }
  }
}
