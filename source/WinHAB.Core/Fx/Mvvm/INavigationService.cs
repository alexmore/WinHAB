using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface INavigationService
  {
    IView CurrentView { get; }

    void NavigateView(IView view); 
    void Navigate(IViewModel viewModel);
    
    IViewModel Navigate(Type viewModelType, Action<ConstructorParameters> ctorParameters);
    IViewModel Navigate(Type viewModelType);

    T Navigate<T>(Action<ConstructorParameters> ctorParameters) where T : IViewModel;
    T Navigate<T>() where T : IViewModel;

    void ClearHistory();
    bool CanGoBack();
    void GoBack();

    RelayCommand GoBackCommand { get; }

    Task ShowMessageAsync(string title, string text);
    void ShowMessage(string title, string text, Action onClose);
  }
}
