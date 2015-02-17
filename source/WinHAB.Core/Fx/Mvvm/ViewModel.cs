using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Fx.Mvvm
{
  public class ViewModel : ViewModelBase, IViewModel
  {
    protected ViewModel(INavigationService navigationService)
    {
      Navigation = navigationService;
    }

    TaskProgressViewModel _taskProgress = new TaskProgressViewModel();
    public TaskProgressViewModel TaskProgress
    {
      get { return _taskProgress; }
      set { _taskProgress = value; RaisePropertyChanged(() => TaskProgress); }
    }

    public INavigationService Navigation { get; protected set; }
    
    public virtual Task InitializeAsync(dynamic parameter)
    {
      return Task.FromResult(0);
    }
  }
}