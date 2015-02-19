using System.Threading.Tasks;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels.Pages
{
  public class ImageWidgetPageModel : PageModelBase
  {
    public ImageWidgetPageModel(INavigationService navigationService) : base(navigationService)
    {
     
    }

    public ImageWidgetModel Image { get; set; }

    public override Task InitializeAsync(dynamic parameter)
    {
      Image = parameter as ImageWidgetModel;
      RaisePropertyChanged(()=>Image);
      return Task.FromResult(default(object));
    }
  }
}