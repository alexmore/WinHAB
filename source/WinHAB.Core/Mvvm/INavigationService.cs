using System;
using System.Threading.Tasks;

namespace WinHAB.Core.Mvvm
{
  public interface INavigationService
  {
    void Navigate(ViewModel viewModel);
    ViewModel Navigate(Type viewModelType, Action<ConstructorParameters> ctorParameters);
    T Navigate<T>() where T : ViewModel;
    T Navigate<T>(Action<ConstructorParameters> ctorParameters) where T : ViewModel;

    void ClearHistory();
    bool CanGoBack();
    void GoBack();

    Task ShowMessageAsync(string title, string text);
    void ShowMessage(string title, string text, Action onClose);
  }
}
