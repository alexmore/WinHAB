using System;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface IViewFactory
  {
    IView Create(Type viewModelType);
  }
}