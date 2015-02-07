using System;
using System.Threading.Tasks;

namespace WinHAB.Core.Mvvm
{
  public interface INavigationService
  {
    void Navigate(IView view); 
    void Navigate(IViewModel viewModel);
    
    ViewModel Navigate(Type viewModelType, Action<ConstructorParameters> ctorParameters);
    void Navigate(Type viewModelType);

    T Navigate<T>(Action<ConstructorParameters> ctorParameters) where T : IViewModel;
    T Navigate<T>() where T : IViewModel;

    void ClearHistory();
    bool CanGoBack();
    void GoBack();

    Task ShowMessageAsync(string title, string text);
    void ShowMessage(string title, string text, Action onClose);
  }
}
