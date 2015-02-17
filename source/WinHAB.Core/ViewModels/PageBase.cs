using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Core.ViewModels
{
  public class PageBase : ViewModel
  {
    protected PageBase(INavigationService navigationService)
    {
      Navigation = navigationService;
    }

    public INavigationService Navigation { get; protected set; }
  }
}