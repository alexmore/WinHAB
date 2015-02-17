﻿using System;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface IViewModelViewFactory : IViewModelViewMapper
  {
    IViewModel CreateViewModel(Type viewModelType);
    IView CreateView(Type viewType);
    IView CreateViewByViewModelType(Type viewModelType);
  }

}