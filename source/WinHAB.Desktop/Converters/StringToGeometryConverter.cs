using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WinHAB.Core.Fx;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Assets;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;

namespace WinHAB.Desktop.Converters
{
  public class StringToGeometryConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;

      var iconString = value.ToString();

      Geometry result = null;

      result = GetIconGeometry(iconString.Replace("-", ""));
      if (result == null)
      {
        if (iconString.LastIndexOf("-") >= 0)
          result = GetIconGeometry(iconString.Substring(0, iconString.LastIndexOf("-")));
      }


      if (result == null && parameter != null) result = parameter as Geometry;

      return result;
    }


    Geometry GetIconGeometry(string iconString)
    {
      if (iconString.IsNullOrWhitespace()) return null;

      var resourceKey = UserResources.Icons.GetResourceKeys().FirstOrDefault(x =>
        x.ToLower() == iconString.ToLower().Trim() ||
        x.ToLower() == (iconString.Trim() + "icon").ToLower());

      if (resourceKey == null)
      {
        var g = Application.Current.TryFindResource(iconString) as Geometry;
        return g;
      }

      return resourceKey != null ? UserResources.Icons[resourceKey] as Geometry : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return null;
    }

  }
}