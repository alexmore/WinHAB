using System;

namespace WinHAB.Core.Mvvm
{
  public interface IViewModelViewMapper
  {
    IViewModelViewMapper Map(Type viewModelType, Type viewType);

    IViewModelViewMapper Map<TViewModel, TView>()
      where TViewModel : IViewModel
      where TView : IView; 
  }
}