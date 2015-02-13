using WinHAB.Core.Model;

namespace WinHAB.Core.ViewModels.Widgets
{
  public interface IWidgetsFactory
  {
    WidgetBase Create(WidgetData data);
  }
}