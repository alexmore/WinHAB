using System;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class WidgetsFactory
  {
    private readonly Func<Type, Widget, WidgetBase> _createWidgetFunc;

    public WidgetsFactory(Func<Type, Widget, WidgetBase> createWidgetFunc)
    {
      _createWidgetFunc = createWidgetFunc;
    }

    public WidgetBase Create(Widget data)
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
          return _createWidgetFunc(typeof (ImageWidget), data);
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