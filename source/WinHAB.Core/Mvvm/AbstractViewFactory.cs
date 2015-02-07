using System;
using System.Collections.Generic;
using System.Linq;

namespace WinHAB.Core.Mvvm
{
  public abstract class AbstractViewFactory : IViewFactory
  {
    private readonly List<KeyValuePair<Type, Type>> _map = new List<KeyValuePair<Type, Type>>();

    public IViewBinder Bind<TViewModel, TView>() where TViewModel : IViewModel where TView : IView
    {
      _map.Add(new KeyValuePair<Type, Type>(typeof (TViewModel), typeof (TView)));
      
      return this;
    }

    public abstract IView Create(Type viewType);

    public virtual T Create<T>() where T : IView
    {
      return (T)Create(typeof (T));
    }

    public virtual IView CreateForViewModel(Type viewModelType)
    {
      var viewType = _map.Where(x => x.Key == viewModelType).Select(x=>x.Value).FirstOrDefault();
      if (viewType != null) return Create(viewType);

      return null;
    }

    public virtual IView CreateForViewModel<TViewModel>() where TViewModel : IViewModel
    {
      return CreateForViewModel(typeof (TViewModel));
    }

  }
}