using WinHAB.Core.Models;

namespace WinHAB.Core.ViewModels
{
  public interface IWidgetsFactory
  {
    WidgetModelBase Create(Widget data);
  }
}