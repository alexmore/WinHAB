using System;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class WidgetsFactory : IWidgetsFactory
  {
    private readonly Func<Type, Widget, WidgetModelBase> _createWidgetFunc;

    public WidgetsFactory(Func<Type, Widget, WidgetModelBase> createWidgetFunc)
    {
      _createWidgetFunc = createWidgetFunc;
    }

    public WidgetModelBase Create(Widget data)
    {
      switch (data.Type)
      {
        case WidgetType.Unknown:
          return null;
        case WidgetType.Group:
          return _createWidgetFunc(typeof(TextWidgetModel), data);
        case WidgetType.Frame:
          return _createWidgetFunc(typeof (FrameWidgetModel), data);
        case WidgetType.Image:
          return _createWidgetFunc(typeof (ImageWidgetModel), data);
        case WidgetType.Selection:
          return _createWidgetFunc(typeof(SelectionWidgetModel), data);
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
          return CreateSwitchWidget(data);
        case WidgetType.Colorpicker:
          break;
        case WidgetType.Text:
          return _createWidgetFunc(typeof(TextWidgetModel), data);
        default:
          return null;
      }

      return null;
    }

    WidgetModelBase CreateSwitchWidget(Widget data)
    {
      if (data.Item != null && data.Item.Type == ItemType.Rollershutter)
      {
        // TODO: return Rollershutter widget
        return null;
      }

      if (data.Mappings != null && data.Mappings.Count > 1)
      {
        return _createWidgetFunc(typeof(SelectionWidgetModel), data);
      }

      return _createWidgetFunc(typeof(SwitchWidgetModel), data);
    }
  }
}