using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Fx.Mvvm
{
  public class ViewModel : ViewModelBase, IViewModel
  {
    public virtual Task InitializeAsync(object parameter)
    {
      return Task.FromResult(0);
    }

    #region Progress indicator tools

    private bool _isProgressIndicatorVisible;
    public bool IsProgressIndicatorVisible
    {
      get { return _isProgressIndicatorVisible; }
      set { _isProgressIndicatorVisible = value; RaisePropertyChanged(() => IsProgressIndicatorVisible); }
    }

    public void ShowProgressIndicator()
    {
      IsProgressIndicatorVisible = true;
    }

    public void HideProgressIndicator()
    {
      IsProgressIndicatorVisible = false;
    }
    #endregion
  }
}