using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace WinHAB.Core.Fx.Mvvm
{
  public abstract class NavigationServiceBase : INavigationService
  {
    public Stack<IView> History { get; set; }

    protected IViewFactory Factory { get; set; }

    protected NavigationServiceBase(IViewFactory factory)
    {
      Factory = factory;
      History = new Stack<IView>();
      GoBackCommand = new RelayCommand(GoBack);
    }

    public IView CurrentView { get; protected set; }

    public abstract void NavigateView(IView view);
    
    public abstract Task ShowMessageAsync(string title, string text);
    public abstract void ShowMessage(string title, string text, Action onClose);
    
    public virtual async Task<IViewModel> NavigateAsync(Type viewModelType, dynamic parameter)
    {
      var view = Factory.Create(viewModelType);
      if (view == null)
        throw new ArgumentException("Fails to create instance of view for view model" + viewModelType + ". May be view doesn't have ViewModelAttribute.");
      
      var vm = view.DataContext as IViewModel;
      if (vm == null)
        throw new ArgumentException("Fails to create instance of view model "+viewModelType+". View model must be passed as constructor parameter to a view and assign to DataContext property.");

      if (CurrentView != null) History.Push(CurrentView);
      NavigateView(view);
      CurrentView = view;
      
      await vm.InitializeAsync(parameter);
      
      return vm;
    }

    public virtual async Task<T> NavigateAsync<T>(dynamic parameter) where T : IViewModel
    {
      return (T)await NavigateAsync(typeof(T), parameter);
    }

    public virtual async Task<T> NavigateAsync<T>() where T : IViewModel
    {
      return await NavigateAsync<T>(null);
    }

    public virtual void ClearHistory()
    {
      while (History.Count > 0)
        History.Pop().CleanupView();

      if (CurrentView != null)
      {
        CurrentView.CleanupView();
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
        CurrentView.CleanupView();
        CurrentView = null;
      }

      CurrentView = History.Pop(); 
      NavigateView(CurrentView);
    }

    public RelayCommand GoBackCommand { get; protected set; }
  }
}