using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Assets;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;
using WinHAB.Desktop.Windows;

namespace WinHAB.Desktop.Converters
{
  public class StringToGeometryConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;

        var stringKey = value.ToString().Replace("-","");

        var resourceKey = UserResources.Icons.GetResourceKeys().FirstOrDefault(x =>
          x.ToLower() == stringKey.ToLower().Trim() ||
          x.ToLower() == (stringKey.Trim() + "icon").ToLower());

        return resourceKey != null ? UserResources.Icons[resourceKey] as Geometry : null;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        return null;
      }
     
  }
}