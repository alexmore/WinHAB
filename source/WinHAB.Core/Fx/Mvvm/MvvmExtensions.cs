using System;

namespace WinHAB.Core.Fx.Mvvm
{
  public static class MvvmExtensions
  {
    public static void Cleanup(this IView view)
    {
      if (view == null) return;
      var vm = view.DataContext as IViewModel;
      if (vm == null) return;
      vm.Cleanup();
    }
  }
}