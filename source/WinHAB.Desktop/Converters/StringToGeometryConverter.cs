using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Windows;

namespace WinHAB.Desktop.Converters
{
  public class StringToGeometryConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;

        var geometryName = value.ToString();

        return UserResources.Icons.FindIconResource(geometryName) as Geometry ?? 
          new ResourceDictionary()
            {Source = new Uri("/Themes/Icons.xaml", UriKind.Relative)}
              .FindIconResource(geometryName) as Geometry;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        return null;
      }
     
  }
}