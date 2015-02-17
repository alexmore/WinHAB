using System;

namespace WinHAB.Core.Fx.Mvvm
{
  public static class MvvmExtensions
  {
    public static ConstructorParameters GetConstructorParameters(this Action<ConstructorParameters> ctorParametersAction)
    {
      var ctorp = new ConstructorParameters();
      if (ctorParametersAction != null) ctorParametersAction(ctorp);
      return ctorp;
    }

    public static void Cleanup(this IView view)
    {
      if (view == null) return;
      var vm = view.DataContext as IViewModel;
      if (vm == null) return;
      vm.Cleanup();
    }
  }
}