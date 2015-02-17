using System;
using System.Collections.Generic;
using System.Linq;

namespace WinHAB.Core.Fx.Mvvm
{
  public abstract class AbstractViewModelViewFactory : IViewModelViewFactory
  {
    protected readonly List<KeyValuePair<Type, Type>> MapList = new List<KeyValuePair<Type, Type>>();
    
    public abstract IViewModel CreateViewModel(Type viewModelType);
    public abstract IView CreateView(Type viewType);

    public IView CreateViewByViewModelType(Type viewModelType)
    {
      var viewType = MapList.Where(x => x.Key == viewModelType).Select(x => x.Value).FirstOrDefault();

      if (viewType == null) return null;

      return CreateView(viewType);
    }

    public IViewModelViewMapper Map(Type viewModelType, Type viewType)
    {
      MapList.Add(new KeyValuePair<Type, Type>(viewModelType, viewType));
      return this;
    }

    public IViewModelViewMapper Map<TViewModel, TView>()
      where TViewModel : IViewModel
      where TView : IView
    {
      Map(typeof(TViewModel), typeof(TView));

      return this;
    }
  }
}