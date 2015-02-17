using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Ninject;
using Ninject.Parameters;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Desktop.Configuration
{
  public class DesktopViewModelViewFactory : AbstractViewModelViewFactory
  {
    private readonly IKernel _kernel;

    public DesktopViewModelViewFactory(IKernel kernel)
    {
      _kernel = kernel;
    }

    public override IViewModel CreateViewModel(Type viewModelType)
    {
      return (IViewModel) _kernel.Get(viewModelType);
    }

    public override IView CreateView(Type viewType)
    {
      return (IView)_kernel.Get(viewType);
    }
  }
}