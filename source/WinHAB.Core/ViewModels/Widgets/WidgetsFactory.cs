using System;
using WinHAB.Core.Model;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class WidgetsFactory : IWidgetsFactory
  {
    private readonly Func<Type, WidgetData, WidgetBase> _createWidgetFunc;

    public WidgetsFactory(Func<Type, WidgetData, WidgetBase> createWidgetFunc)
    {
      _createWidgetFunc = createWidgetFunc;
    }

    public WidgetBase Create(WidgetData data)
    {
      switch (data.Type)
      {
        case WidgetType.Unknown:
          return null;
        case WidgetType.Group:
          return _createWidgetFunc(typeof(TextWidget), data);
        case WidgetType.Frame:
          return _createWidgetFunc(typeof (FrameWidget), data);
        case WidgetType.Image:
          break;
        case WidgetType.Selection:
          break;
        case WidgetType.Slider:
          break;
        case WidgetType.Chart:
          break;
        case WidgetType.Video:
          break;
        case WidgetType.Webview:
          break;
        case WidgetType.Setpoint:
          break;
        case WidgetType.Switch:
          break;
        case WidgetType.Colorpicker:
          break;
        case WidgetType.Text:
          return _createWidgetFunc(typeof(TextWidget), data);
        default:
          return null;
      }

      return null;
    }
  }
}