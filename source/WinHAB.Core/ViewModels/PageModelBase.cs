using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Core.ViewModels
{
  public class PageModelBase : ViewModel
  {
    protected PageModelBase(INavigationService navigationService)
    {
      Navigation = navigationService;
    }

    public INavigationService Navigation { get; protected set; }
  }
}