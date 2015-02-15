﻿using System.Collections.ObjectModel;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class FrameWidget : WidgetBase
  {
    public FrameWidget(INavigationService navigationService, WidgetData data) : base(navigationService, data)
    {
      Widgets = new ObservableCollection<WidgetBase>();
    }

    private ObservableCollection<WidgetBase> _Widgets;
    public ObservableCollection<WidgetBase> Widgets { get { return _Widgets; } set { _Widgets = value; RaisePropertyChanged(() => Widgets); } }
  }
}