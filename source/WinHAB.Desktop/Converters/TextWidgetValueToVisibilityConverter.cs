using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Desktop.Converters
{
  public class TextWidgetValueToVisibilityConverter : IValueConverter
  {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
        var widget = value as TextWidget;
        if (widget == null) return Visibility.Collapsed;

        parameter = parameter ?? "";
        
        var isShortValue = string.IsNullOrWhiteSpace(widget.Value) || widget.Value.Length < 9;

        if (parameter.ToString() == "icon")
          return isShortValue ? Visibility.Visible : Visibility.Collapsed;
        
        return !isShortValue ? Visibility.Visible : Visibility.Collapsed;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
        return null;
      }
     
  }
}