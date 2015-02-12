﻿using System;
using System.Collections.Generic;
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
using WinHAB.Core.Mvvm;

namespace WinHAB.Desktop.Views
{
  /// <summary>
  /// Interaction logic for MainView.xaml
  /// </summary>
  public partial class MainView : IView
  {
    public MainView()
    {
      InitializeComponent();
    }
    
    private void HorizontalScroll(object sender, MouseWheelEventArgs e)
    {
      var scrollViewer = (sender as ScrollViewer);
      if (scrollViewer == null) return;

      scrollViewer.ScrollToHorizontalOffset(scrollViewer.ContentHorizontalOffset + e.Delta * -1);
    }
  }
}
