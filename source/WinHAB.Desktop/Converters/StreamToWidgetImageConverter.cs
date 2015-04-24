﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Controls;

namespace WinHAB.Desktop.Converters
{
  public class StreamToWidgetImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var imageStream = value as System.IO.Stream;

      if (imageStream != null)
      {
        try
        {
          var image = new System.Windows.Media.Imaging.BitmapImage();
          image.BeginInit();
          image.StreamSource = imageStream;
          image.EndInit();
          
          var tr = new ScaleTransform();

          double scale = 1;

          var tileW = WidgetsPanel.WidgetLargeWidth;
          var tileH = WidgetsPanel.WidgetLargeHeight;

          if (image.Height < image.Width)
            scale = tileH/image.Height;
          else
            scale = tileW / image.Width;

          var transform = new ScaleTransform(scale, scale, .5, .5);

          var ti = new TransformedBitmap();
          ti.BeginInit();
          ti.Source = image;
          ti.Transform = transform;
          ti.EndInit();

          return ti;
        }
        catch
        {
        }
      }
      
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}