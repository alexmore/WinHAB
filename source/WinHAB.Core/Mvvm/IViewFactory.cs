using System;

namespace WinHAB.Core.Mvvm
{
  public interface IViewFactory : IViewBinder
  {
    IView Create(Type viewType);
    T Create<T>() where T : IView;

    IView CreateForViewModel(Type viewModelType);
    IView CreateForViewModel<TViewModel>() where TViewModel : IViewModel;
  }

  public interface IViewBinder
  {
    IViewBinder Bind<TViewModel, TView>() where TViewModel : IViewModel where TView : IView;
  }
}