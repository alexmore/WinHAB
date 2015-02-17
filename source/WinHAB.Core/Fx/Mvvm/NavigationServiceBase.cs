using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace WinHAB.Core.Fx.Mvvm
{
  public abstract class NavigationServiceBase : INavigationService
  {
    protected Stack<IView> History { get; set; }

    protected IViewModelViewFactory Factory { get; set; }

    protected NavigationServiceBase(IViewModelViewFactory factory)
    {
      Factory = factory;
      History = new Stack<IView>();
      GoBackCommand = new RelayCommand(GoBack);
    }

    public IView CurrentView { get; protected set; }

    public abstract void NavigateView(IView view);
    
    public abstract Task ShowMessageAsync(string title, string text);
    public abstract void ShowMessage(string title, string text, Action onClose);
    

    public virtual async Task NavigateAsync(IViewModel viewModel, dynamic parameter)
    {
      var view = Factory.CreateViewByViewModelType(viewModel.GetType());
      view.DataContext = viewModel;
      if (CurrentView != null) History.Push(CurrentView);
      NavigateView(view);
      CurrentView = view;
      await viewModel.InitializeAsync(parameter);
    }

    public virtual async Task<IViewModel> NavigateAsync(Type viewModelType, dynamic parameter)
    {
      var vm = Factory.CreateViewModel(viewModelType);
      await NavigateAsync(vm, parameter);
      return vm;
    }

    public virtual async Task<T> NavigateAsync<T>(dynamic parameter) where T : IViewModel
    {
      return (T)await NavigateAsync(typeof(T), parameter);
    }

    public virtual async Task<T> NavigateAsync<T>() where T : IViewModel
    {
      return (T) await NavigateAsync(typeof(T), null);
    }

    public virtual void ClearHistory()
    {
      while (History.Count > 0)
        History.Pop().Cleanup();

      if (CurrentView != null)
      {
        CurrentView.Cleanup();
        CurrentView = null;
      }
    }

    public virtual bool CanGoBack()
    {
      return History.Count > 0;
    }

    public virtual void GoBack()
    {
      if (!CanGoBack()) return;

      if (CurrentView != null)
      {
        CurrentView.Cleanup();
        CurrentView = null;
      }

      CurrentView = History.Pop(); 
      NavigateView(CurrentView);
    }

    public RelayCommand GoBackCommand { get; protected set; }
  }
}