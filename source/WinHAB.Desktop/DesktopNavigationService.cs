using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using WinHAB.Core.Mvvm;

namespace WinHAB.Desktop
{
  public class DesktopNavigationService : INavigationService
  {
    private readonly Stack<IView> _history = new Stack<IView>();

    private readonly INavigationHost _navigationHost;
    private readonly IViewModelViewFactory _factory;

    public DesktopNavigationService(INavigationHost navigationHost, IViewModelViewFactory factory)
    {
      _navigationHost = navigationHost;
      _factory = factory;
    }

    public IView CurrentVeiw { get; private set; }

    public void Navigate(IView view)
    {
      if (CurrentVeiw != null) _history.Push(CurrentVeiw);
      _navigationHost.Content = null;
      _navigationHost.Content = view;
      CurrentVeiw = view;
    }

    public void Navigate(IViewModel viewModel)
    {
      var view = _factory.CreateViewByViewModelType(viewModel.GetType());
      view.DataContext = viewModel;
      Navigate(view);
      viewModel.OnNavigatedTo();
    }

    public IViewModel Navigate(Type viewModelType, Action<ConstructorParameters> ctorParameters)
    {
      var vm = _factory.CreateViewModel(viewModelType, ctorParameters);
      Navigate(vm);
      return vm;
    }

    public IViewModel Navigate(Type viewModelType)
    {
      var vm = _factory.CreateViewModel(viewModelType, null);
      Navigate(vm);
      return vm;
    }

    public T Navigate<T>(Action<ConstructorParameters> ctorParameters) where T : IViewModel
    {
      return (T)Navigate(typeof (T), ctorParameters);
    }

    public T Navigate<T>() where T : IViewModel
    {
      return (T)Navigate(typeof (T));
    }
    
    public void ClearHistory()
    {
      while (_history.Count > 0)
        _history.Pop().Cleanup();

      if (CurrentVeiw != null)
      {
        CurrentVeiw.Cleanup();
        CurrentVeiw = null;
      }
    }

    public bool CanGoBack()
    {
      return _history.Count > 0;
    }

    public void GoBack()
    {
      if (CurrentVeiw != null)
      {
        CurrentVeiw.Cleanup();
        CurrentVeiw = null;
      }
      Navigate(_history.Pop());
    }

    public Task ShowMessageAsync(string title, string text)
    {
      var tcs = new TaskCompletionSource<bool>();
      ShowMessage(title, text, () => tcs.SetResult(true));
      return tcs.Task;
    }

    public void ShowMessage(string title, string text, Action onClose)
    {
      ModernDialog.ShowMessage(text, title, MessageBoxButton.OK);
      onClose();
    }
  }
}