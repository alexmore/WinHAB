using WinHAB.Core.Models;

namespace WinHAB.Core.ViewModels.Widgets
{
  public interface IWidgetsFactory
  {
    WidgetBase Create(WidgetData data);
  }
}