using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Desktop.Converters
{
  public class CountToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var result = Visibility.Collapsed;
        if (value != null)
        {
          int cnt = 0;
          if (int.TryParse(value.ToString(), out cnt))
            if (cnt != 0) result = Visibility.Visible;
        }

        if (parameter != null && parameter.ToString() == "inverse")
          result = result == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

        return result;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        return null;
      }
     
  }
}