﻿using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class SwitchWidgetModel : WidgetModelBase
  {
    private readonly INavigationService _navigation;

    public SwitchWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
      : base(data, clientFactory)
    {
      if (data != null && data.Mappings != null && data.Mappings.Count > 1)
        throw new ArgumentException(
          "SwitchWidgetModel can not create instance for Widget with Mappings count greater than 1.");

      _navigation = navigation;
      Size = WidgetSize.Meduim;

      Icon = Data.Icon;

      SetState(data);

      if (!IsButton)
        ItemChanged += (sender, item) =>
        {
          Data.Item = item;
          SetState(Data);
        };

      PostCommand = new AsyncRelayCommand(Post);
    }

    private void SetState(Widget data)
    {
      HideProgressIndicator();

      if (data.Mappings != null && data.Mappings.Count == 1)
      {
        State = SwitchWidgetState.Normal;
        Value = data.Mappings[0].Label;
        IsButton = true;
        return;
      }
      
      State = data.Item.State == "ON" ? SwitchWidgetState.Active : SwitchWidgetState.Inactive;
    }

    private bool _isButton = false;
    public bool IsButton
    {
      get { return _isButton; }
      set { _isButton = value; RaisePropertyChanged(()=>IsButton); }
    }

    private SwitchWidgetState _State;
    public SwitchWidgetState State
    {
      get { return _State; }
      set { _State = value; RaisePropertyChanged(()=>State); }
    }

    public AsyncRelayCommand PostCommand { get; set; }
    string GetCommandString()
    {
      if (IsButton) return Data.Mappings[0].Command;

      return State == SwitchWidgetState.Active ? "OFF" : "ON";
    }

    async Task Post()
    {
      using (var cln = ClientFactory.Create())
      {
        try
        {
          await cln.PostAsync(Data.Item.Link, new StringContent(GetCommandString())).IsOkAsync();
          
          if (!IsButton) ShowProgressIndicator();
        }
        catch (Exception e)
        {
          _navigation.ShowMessage(Localization.Strings.MessageExceptionOnSwitchWidgetPostCommandTitle,
            Localization.Strings.MessageExceptionOnSwitchWidgetPostCommand + ":\r\n" + e.Message, () => { });
        }
      }
    }
  }
}