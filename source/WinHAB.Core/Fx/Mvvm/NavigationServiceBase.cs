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
    

    public virtual void Navigate(IViewModel viewModel)
    {
      var view = Factory.CreateViewByViewModelType(viewModel.GetType());
      view.DataContext = viewModel;
      if (CurrentView != null) History.Push(CurrentView);
      NavigateView(view);
      CurrentView = view;
      viewModel.OnLoaded();
    }

    public virtual IViewModel Navigate(Type viewModelType, Action<ConstructorParameters> ctorParameters)
    {
      var vm = Factory.CreateViewModel(viewModelType, ctorParameters);
      Navigate(vm);
      return vm;
    }

    public virtual IViewModel Navigate(Type viewModelType)
    {
      var vm = Factory.CreateViewModel(viewModelType, null);
      Navigate(vm);
      return vm;
    }

    public virtual T Navigate<T>(Action<ConstructorParameters> ctorParameters) where T : IViewModel
    {
      return (T)Navigate(typeof(T), ctorParameters);
    }

    public virtual T Navigate<T>() where T : IViewModel
    {
      return (T)Navigate(typeof(T));
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