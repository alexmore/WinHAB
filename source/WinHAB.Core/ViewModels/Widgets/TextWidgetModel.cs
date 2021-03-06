﻿using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class TextWidgetModel : WidgetModelBase, INavigationWidget
  {
    public TextWidgetModel(Widget data, IRestClientFactory clientFactory) : base(data, clientFactory)
    {
      if (Data != null && Data.Properties != null)
        Size = Data.Properties.GetSize() ?? WidgetSize.Meduim;
    }

    public AsyncRelayCommand<WidgetModelBase> NavigateLinkedPageCommand { get; set; }
  }
}