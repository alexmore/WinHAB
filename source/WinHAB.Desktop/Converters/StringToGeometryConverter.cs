using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Desktop.Converters
{
  public class StringToGeometryConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;

        var geometry = value.ToString();
      
        if (!geometry.StartsWith("M") && !geometry.EndsWith("z"))
        {
          var resource = new ResourceDictionary
          {
            Source = new Uri("/Themes/Icons.xaml", UriKind.Relative)
          };
          var resKey =
            resource.Keys.Cast<string>()
              .FirstOrDefault(x => x.ToLower() == (geometry + "Icon").ToLower());
          if (resKey != null)
            return (Geometry) Application.Current.Resources[resKey];
        }

        if (geometry.StartsWith("M") && geometry.EndsWith("z"))
        {
          try
          {
            return Geometry.Parse(geometry);
          }
          catch
          {
          }
        }

        return null;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        return null;
      }
     
  }
}