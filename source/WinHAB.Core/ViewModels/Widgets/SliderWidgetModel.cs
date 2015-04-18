using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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

    public SliderWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
      : base(data, clientFactory)
    {
      _navigation = navigation;

      IsSwitchSupport = Data.SwitchSupport;

      Size = Data.Properties.Values.Size ?? WidgetSize.Meduim;
      Size = Size == WidgetSize.Large ? WidgetSize.Wide : Size;


      Levels = new ObservableCollection<LevelItem>(new[]
        {
          new LevelItem(100), new LevelItem(90), new LevelItem(80), new LevelItem(70), new LevelItem(60),
          new LevelItem(50), new LevelItem(40), new LevelItem(30), new LevelItem(20), new LevelItem(10)
        });

      SetValue(Data.Item.IsNotNull() ? Data.Item.State : null);

      ItemChanged += (sender, item) =>
      {
        Data.Item = item;
        SetValue(Data.Item.State);
      };

      SetPercentCommand = new AsyncRelayCommand<int>(SetPercent);
      OnOffCommand = new AsyncRelayCommand(OnOff);
      OnCommand = new AsyncRelayCommand(On);
      OffCommand = new AsyncRelayCommand(Off);
      IncreaseCommand = new AsyncRelayCommand(Increase);
      DecreaseCommand = new AsyncRelayCommand(Decrease);
    }

    #region State
    private decimal? PercentValue { get; set; }

    private void SetValue(string itemState)
    {
      HideProgressIndicator();
      PercentValue = itemState.ToDecimal();
      if (PercentValue.HasValue)
      {
        Value = _valueFormat.FormatString(PercentValue);
        foreach (var l in Levels)
        {
          if (l.Percent <= PercentValue) l.IsChecked = true;
          else l.IsChecked = false;
        }
      }
      else
        Value = null;

      RaisePropertyChanged(() => IsOff);
    }

    public bool IsOff { get { return !PercentValue.HasValue || PercentValue.Value == 0; } }
    
    public bool IsSwitchSupport { get; set; }
    #endregion

    #region Commands
    public AsyncRelayCommand<int> SetPercentCommand { get; set; }
    public AsyncRelayCommand OnOffCommand { get; set; }
    public AsyncRelayCommand OffCommand { get; set; }
    public AsyncRelayCommand OnCommand { get; set; }
    public AsyncRelayCommand IncreaseCommand { get; set; }
    public AsyncRelayCommand DecreaseCommand { get; set; }


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

    async Task Increase()
    {
      await PostValue("INCREASE");
    }

    async Task Decrease()
    {
      await PostValue("DECREASE");
    }

    async Task SetPercent(int percent)
    {
      await PostValue(percent.ToString());
    }
    #endregion

    #region Level

    public class LevelItem : ObservableObject
    {
      public LevelItem(int percent)
      {
        Percent = percent;
      }

      public int Percent { get; set; }

      private bool _isChecked;
      public bool IsChecked
      {
        get { return _isChecked; }
        set { _isChecked = value; RaisePropertyChanged(() => IsChecked); }
      }
    }

    private ObservableCollection<LevelItem> _levels;
    public ObservableCollection<LevelItem> Levels
    {
      get { return _levels; }
      set { _levels = value; }
    }
    #endregion

  }
}