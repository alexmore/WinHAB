using System;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Desktop.Fx.Mvvm
{
  public class DesktopNavigationService : NavigationServiceBase
  {
    private readonly INavigationHost _navigationHost;

    public DesktopNavigationService(INavigationHost navigationHost, IViewFactory factory)
      : base(factory)
    {
      _navigationHost = navigationHost;
    }

    public override void NavigateView(IView view)
    {
      _navigationHost.Content = null;
      _navigationHost.Content = view;
    }

    public override Task ShowMessageAsync(string title, string text)
    {
      var tcs = new TaskCompletionSource<bool>();
      ShowMessage(title, text, () => tcs.SetResult(true));
      return tcs.Task;
    }

    public override Task<bool> ShowQuestionAsync(string title, string text)
    {
      var tcs = new TaskCompletionSource<bool>();
      if (ModernDialog.ShowMessage(text, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        tcs.SetResult(true);
      else
        tcs.SetResult(false);
      
      return tcs.Task;
    }

    public override void ShowMessage(string title, string text, Action onClose)
    {
      ShowMessage(title, text, onClose, MessageBoxButton.OK);
    }

    private void ShowMessage(string title, string text, Action onClose, MessageBoxButton buttons)
    {
      ModernDialog.ShowMessage(text, title, buttons);
      onClose();
    }
  }
}