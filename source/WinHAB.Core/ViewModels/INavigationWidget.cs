using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Core.ViewModels
{
  public interface INavigationWidget
  {
    AsyncRelayCommand<WidgetModelBase> NavigateLinkedPageCommand { get; set; }
  }
}