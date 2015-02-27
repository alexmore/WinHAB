using System.Collections.Generic;
using System.Linq;
using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Models
{
  public static class ModelExtensions
  {
    public static Widget[] CreateFrameWrappedCollection(this IEnumerable<Widget> widgets)
    {
      if (widgets == null) return null;
      if (!widgets.Any()) return new Widget[0];

      const string fakeFrameLabel = "#WRAPPER#";

      var frames = new List<Widget>();
      foreach (var widget in widgets)
      {
        if (widget.Type == WidgetType.Frame) frames.Add(widget);
        else
        {
          if (frames.Count == 0 || frames.Last() == null || frames.Last().Label != fakeFrameLabel)
            frames.Add(new Widget() { Widgets = new List<Widget>(), Label = fakeFrameLabel, Type = WidgetType.Frame });
          frames.Last().Widgets.Add(widget);
        }
      }

      foreach (var frame in frames.Where(x => x.Label == fakeFrameLabel))
        frame.Label = null;

      return frames.ToArray();
    }
  }
}