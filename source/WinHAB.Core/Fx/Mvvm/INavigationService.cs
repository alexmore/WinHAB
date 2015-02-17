using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface INavigationService
  {
    IView CurrentView { get; }

    void NavigateView(IView view); 
    Task NavigateAsync(IViewModel viewModel, dynamic parameter);
    
    Task<IViewModel> NavigateAsync(Type viewModelType, dynamic parameter);

    Task<T> NavigateAsync<T>(dynamic parameter) where T : IViewModel;
    Task<T> NavigateAsync<T>() where T : IViewModel;

    void ClearHistory();
    bool CanGoBack();
    void GoBack();

    RelayCommand GoBackCommand { get; }

    Task ShowMessageAsync(string title, string text);
    void ShowMessage(string title, string text, Action onClose);
  }
}
