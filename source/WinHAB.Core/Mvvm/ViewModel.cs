

using GalaSoft.MvvmLight;

namespace WinHAB.Core.Mvvm
{
  public class ViewModel : ViewModelBase, IViewModel
  {
    protected ViewModel(INavigationService navigationService)
    {
      Navigation = navigationService;
    }

    WaiterViewModel _waiter = new WaiterViewModel();
    public WaiterViewModel Waiter
    {
      get { return _waiter; }
      set { _waiter = value; RaisePropertyChanged(() => Waiter); }
    }

    public INavigationService Navigation { get; protected set; }
    
    public virtual void OnNavigatedTo()
    {
    }
  }
}