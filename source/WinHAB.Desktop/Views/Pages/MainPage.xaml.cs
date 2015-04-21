using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Desktop.Views.Pages
{
  /// <summary>
  /// Interaction logic for MainView.xaml
  /// </summary>
  [ViewModel(typeof(MainPageModel))]
  public partial class MainPage : IView
  {
    public MainPage()
    {
      InitializeComponent();

      this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      var dependancyProperyDescriptor = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof (ItemsControl));

      if (dependancyProperyDescriptor != null)
      {
        dependancyProperyDescriptor.AddValueChanged(Widgets, ItemsSourceChangedEvent);
      }
    }

    private void ItemsSourceChangedEvent(object sender, EventArgs e)
    {
      Transition.StartTransition();
    }

    private void HorizontalScroll(object sender, MouseWheelEventArgs e)
    {
      var scrollViewer = (sender as ScrollViewer);
      if (scrollViewer == null) return;

      scrollViewer.ScrollToHorizontalOffset(scrollViewer.ContentHorizontalOffset + e.Delta * -1);
    }
  }
}
