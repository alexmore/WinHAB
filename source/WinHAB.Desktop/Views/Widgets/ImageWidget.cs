using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WinHAB.Desktop.Views.Widgets
{
  public class ImageWidget : Button
  {

    public static readonly DependencyProperty GotoChildPageCommandProperty = DependencyProperty.Register(
      "GotoChildPageCommand", typeof (ICommand), typeof (ImageWidget), new PropertyMetadata(default(ICommand)));

    public ICommand GotoChildPageCommand
    {
      get { return (ICommand) GetValue(GotoChildPageCommandProperty); }
      set { SetValue(GotoChildPageCommandProperty, value); }
    }

    public static readonly DependencyProperty GotoChildPageCommandParameterProperty = DependencyProperty.Register(
      "GotoChildPageCommandParameter", typeof (object), typeof (ImageWidget), new PropertyMetadata(default(object)));

    public object GotoChildPageCommandParameter
    {
      get { return (object) GetValue(GotoChildPageCommandParameterProperty); }
      set { SetValue(GotoChildPageCommandParameterProperty, value); }
    }
  }
}