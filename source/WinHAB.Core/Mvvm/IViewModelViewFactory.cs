using System;

namespace WinHAB.Core.Mvvm
{
  public interface IViewModelViewFactory : IViewModelViewMapper
  {
    IViewModel CreateViewModel(Type viewModelType);
    IViewModel CreateViewModel(Type viewModelType, Action<ConstructorParameters> ctorParameters);
    T CreateViewModel<T>() where T : IViewModel;
    T CreateViewModel<T>(Action<ConstructorParameters> ctorParameters);
    
    
    IView CreateView(Type viewType);
    T CreateView<T>() where T : IView;

    IView CreateForViewModel(Type viewModelType);
    IView CreateForViewModel<TViewModel>() where TViewModel : IViewModel;
  }

}