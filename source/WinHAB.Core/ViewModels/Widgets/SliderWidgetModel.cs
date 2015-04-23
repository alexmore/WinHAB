using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class SliderWidgetModel : WidgetModelBase
  {
    private readonly INavigationService _navigation;
    private const string _valueFormat = "{0:N0}%";
    private const decimal _maxValue = 100;
    private const decimal _minValue = 0;

    public SliderWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
      : base(data, clientFactory)
    {
      _navigation = navigation;

      Size = Data.Properties.Values.Size ?? WidgetSize.Meduim;
      Size = Size == WidgetSize.Large ? WidgetSize.Wide : Size;

      SetValue(Data.Item.IsNotNull() ? Data.Item.State : null);

      ItemChanged += (sender, item) =>
      {
        Data.Item = item;
        SetValue(Data.Item.State);
      };
      
      OnOffCommand = new AsyncRelayCommand(OnOff);
      OnCommand = new AsyncRelayCommand(On);
      OffCommand = new AsyncRelayCommand(Off);
    }

    #region State

    private bool _suspendPost = false;

    private decimal? _PercentValue;
    public decimal? PercentValue { get { return _PercentValue; } set { _PercentValue = value; RaisePropertyChanged(()=>PercentValue);
      if (!_suspendPost) PostValue(value != null ? value.ToString() : "0");
    }}

    readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

    private void SetValue(string itemState)
    {
      _locker.EnterWriteLock();
      try
      {
        _suspendPost = true;
        HideProgressIndicator();
        var state = itemState.ToDecimal();
        if (state.HasValue && state.Value > _maxValue) state = _maxValue;
        if (state.HasValue && state.Value < _minValue) state = _minValue;
        PercentValue = state;
        Value = _valueFormat.FormatString(PercentValue);
        RaisePropertyChanged(() => IsOff);
        _suspendPost = false;
      }
      finally
      {
        _locker.ExitWriteLock();
      }
    }

    public bool IsOff { get { return !PercentValue.HasValue || PercentValue.Value == 0; } }
    
    public bool IsSwitchSupport { get; set; }
    #endregion

    #region Commands
    public AsyncRelayCommand OnOffCommand { get; set; }
    public AsyncRelayCommand OffCommand { get; set; }
    public AsyncRelayCommand OnCommand { get; set; }

    async Task PostValue(string value)
    {
      ShowProgressIndicator();
      try
      {
        await SetItemState(value);
      }
      catch (Exception e)
      {
        HideProgressIndicator();
        _navigation.ShowMessage(Localization.Strings.MessageExceptionOnWidgetSetItemStateTitle,
          Localization.Strings.MessageExceptionOnWidgetSetItemState + ":\r\n" + e.Message, () => { });
      }
    }

    async Task OnOff()
    {
      if (!PercentValue.HasValue || PercentValue.Value == 0)
        await On();
      else
        await Off();
    }

    async Task On()
    {
      await PostValue("ON");
    }

    async Task Off()
    {
      await PostValue("OFF");
    }
    #endregion

  }
}