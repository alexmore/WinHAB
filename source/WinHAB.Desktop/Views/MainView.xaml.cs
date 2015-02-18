﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Desktop.Views
{
  /// <summary>
  /// Interaction logic for MainView.xaml
  /// </summary>
  [ViewModel(typeof(MainPageModel))]
  public partial class MainView : IView
  {
    public MainView()
    {
      InitializeComponent();

      Action setCurrentDateTime = () =>
      {
        TimeTextBlock.Text = DateTime.Now.ToShortTimeString();
        DateTextBlock.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
      };

      setCurrentDateTime();

      var timer = new DispatcherTimer();
      timer.Interval = new TimeSpan(0,0,0,1);
      timer.Tick += (sender, args) => setCurrentDateTime();
      timer.Start();

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
