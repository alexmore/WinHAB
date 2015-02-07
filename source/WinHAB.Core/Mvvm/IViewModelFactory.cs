using System;

namespace WinHAB.Core.Mvvm
{
  public interface IViewModelFactory
  {
    IViewModel Create(Type viewModelType);
    IViewModel Create(Type viewModelType, Action<ConstructorParameters> ctorParameters);
    T Create<T>() where T : IViewModel;
    T Create<T>(Action<ConstructorParameters> ctorParameters);
  }
}