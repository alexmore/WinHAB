﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Desktop
{
  public class DesktopNavigationServiceBase : NavigationServiceBase
  {
    private readonly INavigationHost _navigationHost;

    public DesktopNavigationServiceBase(INavigationHost navigationHost, IViewFactory factory)
      : base(factory)
    {
      _navigationHost = navigationHost;
    }

    public override void NavigateView(IView view)
    {
      _navigationHost.Content = null;
      _navigationHost.Content = view;
    }

    public override Task ShowMessageAsync(string title, string text)
    {
      var tcs = new TaskCompletionSource<bool>();
      ShowMessage(title, text, () => tcs.SetResult(true));
      return tcs.Task;
    }

    public override void ShowMessage(string title, string text, Action onClose)
    {
      ModernDialog.ShowMessage(text, title, MessageBoxButton.OK);
      onClose();
    }
  }
}