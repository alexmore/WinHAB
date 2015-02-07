using System;

namespace WinHAB.Core.Mvvm
{
  public interface IViewModelViewFactory : IViewModelViewMapper
  {
    IViewModel CreateViewModel(Type viewModelType);
    IViewModel CreateViewModel(Type viewModelType, Action<ConstructorParameters> ctorParameters);
    IView CreateView(Type viewType);
    IView CreateViewByViewModelType(Type viewModelType);
  }

}