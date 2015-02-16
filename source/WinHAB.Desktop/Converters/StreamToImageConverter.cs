using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.Converters
{
  public class StreamToImageConverter : IValueConverter
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
          return image;
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