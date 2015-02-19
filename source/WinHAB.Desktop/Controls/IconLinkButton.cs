using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinHAB.Desktop.Controls
{
  public class IconLinkButton : Button
  {

    public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register(
      "IconData", typeof(Geometry), typeof(IconLinkButton), new PropertyMetadata(default(Geometry)));

    public Geometry IconData
    {
      get { return (Geometry)GetValue(IconDataProperty); }
      set { SetValue(IconDataProperty, value); }
    }

    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
      "IconSize", typeof(double), typeof(IconLinkButton), new PropertyMetadata(16D));

    public double IconSize
    {
      get { return (double)GetValue(IconSizeProperty); }
      set { SetValue(IconSizeProperty, value); }
    }
  }
}