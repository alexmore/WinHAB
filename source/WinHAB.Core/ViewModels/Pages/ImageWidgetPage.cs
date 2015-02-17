using System.Threading.Tasks;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels.Pages
{
  public class ImageWidgetPage : PageBase
  {
    public ImageWidgetPage(INavigationService navigationService) : base(navigationService)
    {
     
    }

    public ImageWidget Image { get; set; }

    public override Task InitializeAsync(dynamic parameter)
    {
      Image = parameter as ImageWidget;
      RaisePropertyChanged(()=>Image);
      return Task.FromResult(default(object));
    }
  }
}