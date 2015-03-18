using System;
using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Models
{
  public class WidgetPropertiesValues
  {
    private readonly WidgetProperties _props;

    public WidgetPropertiesValues(WidgetProperties props)
    {
      _props = props;

      if (props == null)
        throw new ArgumentException("WidgetProperties argument can not be null.");
    }

    public WidgetSize? Size { get { return _props.GetSize(); } }
    public bool IsOff { get { return _props.GetIsOff(); } }
    public string Icon { get { return _props.GetIcon(); } }
  }

  public static class WidgetPropertiesExtensions
  {
    public static WidgetSize? GetSize(this WidgetProperties props)
    {
      var sizeString = props["Size"];
      if (sizeString != null)
      {
        switch (sizeString.ToLower())
        {
          case "medium": return WidgetSize.Meduim;
          case "wide": return WidgetSize.Wide;
          case "large": return WidgetSize.Large;
        }
      }

      return null;
    }

    public static bool GetIsOff(this WidgetProperties props)
    {
      var s = props["IsOff"];
      return s != null;
    }

    public static String GetIcon(this WidgetProperties props)
    {
      var s = props["Icon"];
      return s;
    }
  }
}