using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Fx.Mvvm
{
  public class ViewModel : ViewModelBase, IViewModel
  {
    TaskProgressViewModel _taskProgress = new TaskProgressViewModel();
    public TaskProgressViewModel TaskProgress
    {
      get { return _taskProgress; }
      set { _taskProgress = value; RaisePropertyChanged(() => TaskProgress); }
    }

    public virtual Task InitializeAsync(object parameter)
    {
      return Task.FromResult(0);
    }
  }
}