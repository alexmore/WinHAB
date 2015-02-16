using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class ImageWidgetPage : ViewModel
  {
    public ImageWidgetPage(INavigationService navigationService, ImageWidget image) : base(navigationService)
    {
      Image = image;
    }

    public ImageWidget Image { get; set; }
  }
}