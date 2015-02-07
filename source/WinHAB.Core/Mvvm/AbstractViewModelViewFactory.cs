using System;
using System.Collections.Generic;
using System.Linq;

namespace WinHAB.Core.Mvvm
{
  public abstract class AbstractViewModelViewFactory : IViewModelViewFactory
  {
    protected readonly List<KeyValuePair<Type, Type>> MapList = new List<KeyValuePair<Type, Type>>();

    public IViewModelViewMapper Map(Type viewModelType, Type viewType)
    {
      MapList.Add(new KeyValuePair<Type, Type>(viewModelType, viewType));
      return this;
    }

    public IViewModelViewMapper Map<TViewModel, TView>() where TViewModel : IViewModel where TView : IView
    {
      Map(typeof (TViewModel), typeof (TView));

      return this;
    }

    public abstract IViewModel CreateViewModel(Type viewModelType, Action<ConstructorParameters> ctorParameters);

    public virtual IViewModel CreateViewModel(Type viewModelType)
    {
      return CreateViewModel(viewModelType, null);
    }

    public virtual T CreateViewModel<T>() where T : IViewModel
    {
      return (T)CreateViewModel(typeof (T));
    }

    public virtual T CreateViewModel<T>(Action<ConstructorParameters> ctorParameters)
    {
      return (T)CreateViewModel(typeof (T), ctorParameters);
    }

    public abstract IView CreateView(Type viewType);

    public virtual T CreateView<T>() where T : IView
    {
      return (T)CreateView(typeof (T));
    }

    public IView CreateForViewModel(Type viewModelType)
    {
      var viewType = MapList.Where(x => x.Key == viewModelType).Select(x => x.Value).FirstOrDefault();

      if (viewType == null) return null;

      return CreateView(viewType);
    }

    public virtual IView CreateForViewModel<TViewModel>() where TViewModel : IViewModel
    {
      return CreateForViewModel(typeof (TViewModel));
    }
  }
}