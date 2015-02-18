using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using Ninject;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Desktop.Configuration
{
  public class DesktopViewFactory : IViewFactory
  {
    private IKernel _kernel;
    private List<Type> _viewTypes;

    public DesktopViewFactory(IKernel kernel)
    {
      _kernel = kernel;
    }

    public IView Create(Type viewModelType)
    {
      var viewType = _viewTypes
        .FirstOrDefault(x => 
          Attribute.IsDefined(x, typeof (ViewModelAttribute)) && 
          x.GetCustomAttributes(typeof(ViewModelAttribute), true).Cast<ViewModelAttribute>().FirstOrDefault(a=>a.ViewModelType == viewModelType) != null);


      if (viewType != null)
      {
        var v = (IView)_kernel.Get(viewType);
        var vm = _kernel.Get(viewModelType);
        v.DataContext = vm;
        return v;
      }

      return null;
    }

    public void ScanAssembly(Assembly assembly)
    {
      _viewTypes = new List<Type>(assembly.GetTypes().Where(x=>x.GetInterfaces().Contains(typeof(IView)) && Attribute.IsDefined(x, typeof(ViewModelAttribute))));
    }
  }
}