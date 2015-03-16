using WinHAB.Core.ViewModels;

namespace WinHAB.Core.Models
{
  public static class WidgetPropertiesExtensions
  {
    public static WidgetSize? GetSize(this WidgetProperties props)
    {
      var sizeString = props["Size"];
      if (sizeString != null)
      {
        switch (sizeString.ToLower())
        {
          case "medium" : return WidgetSize.Meduim;
          case "wide" : return WidgetSize.Wide;
          case "large" : return WidgetSize.Large;
        }
      }

      return null;
    }
  }
}