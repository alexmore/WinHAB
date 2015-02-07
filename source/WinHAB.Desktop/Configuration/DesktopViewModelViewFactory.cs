using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Ninject;
using Ninject.Parameters;
using WinHAB.Core.Mvvm;

namespace WinHAB.Desktop.Configuration
{
  public class DesktopViewModelViewFactory : AbstractViewModelViewFactory
  {
    private readonly IKernel _kernel;

    public DesktopViewModelViewFactory(IKernel kernel)
    {
      _kernel = kernel;
    }

    public override IViewModel CreateViewModel(Type viewModelType, Action<ConstructorParameters> ctorParameters)
    {
      var ctorArgs = new List<ConstructorArgument>();

      if (ctorParameters != null && ctorParameters.GetConstructorParameters() != null &&
          ctorParameters.GetConstructorParameters().Parameters != null)
      {
        ctorArgs.AddRange(ctorParameters
          .GetConstructorParameters().Parameters
          .Select(x => new ConstructorArgument(x.Key, x.Value)).ToArray());
      }

      return (IViewModel) _kernel.Get(viewModelType, ctorArgs.ToArray());
    }

    public override IView CreateView(Type viewType)
    {
      return (IView)_kernel.Get(viewType);
    }
  }
}