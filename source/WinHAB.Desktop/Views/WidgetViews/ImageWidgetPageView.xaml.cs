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
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Desktop.Views.WidgetViews
{
  /// <summary>
  /// Interaction logic for ImageWidgetPageView.xaml
  /// </summary>
  [ViewModel(typeof(ImageWidgetPageModel))]
  public partial class ImageWidgetPageView : IView
  {
    public ImageWidgetPageView()
    {
      InitializeComponent();
    }
  }
}
