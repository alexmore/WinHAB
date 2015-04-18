using System;
using System.Globalization;
using System.Threading.Tasks;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class SetpointWidgetModel : WidgetModelBase
  {
    private readonly INavigationService _navigation;

    public SetpointWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
      : base(data, clientFactory)
    {
      if (data.MinValue > data.MaxValue)
        throw new ArgumentException(
          "MinValue must be less than MaxValue for SetpointWidget " + data.Title);
      
      if (data.Step == 0)
        data.Step = 1;
      
      Size = Data.Properties.Values.Size ?? WidgetSize.Meduim;
      Size = Size == WidgetSize.Large ? WidgetSize.Wide : Size;

      SetValue(Data.Item.IsNotNull() ? Data.Item.State : null);

      _navigation = navigation;

      UpCommand = new AsyncRelayCommand(Up);
      DownCommand = new AsyncRelayCommand(Down);
      
      ItemChanged += (sender, item) =>
      {
        Data.Item = item;
        SetValue(Data.Item.State);
      };
    }

    private bool IsUnlimitedRange { get { return Data.MaxValue == Data.MinValue; }}

    private decimal _DecimalValue;
    public decimal DecimalValue
    {
      get { return _DecimalValue; }
      set
      {
        _DecimalValue = value;
        RaisePropertyChanged(() => DecimalValue);
        RaisePropertyChanged(() => IsValueMax);
        RaisePropertyChanged(() => IsValueMin);
      }
    }
    
    private void SetValue(string itemState)
    {
      HideProgressIndicator();
      var val = itemState.ToDecimal();
      if (val.HasValue)
      {
        DecimalValue = val.Value;
        Value = DecimalValue.ToString();
      }
    }

    async Task PostValue(decimal value)
    {
      ShowProgressIndicator();
      try
      {
        await SetItemState(value.ToString(CultureInfo.InvariantCulture));
      }
      catch (Exception e)
      {
        HideProgressIndicator();
        _navigation.ShowMessage(Localization.Strings.MessageExceptionOnWidgetSetItemStateTitle,
          Localization.Strings.MessageExceptionOnWidgetSetItemState + ":\r\n" + e.Message, () => { });
      }
    }

    public AsyncRelayCommand UpCommand { get; set; }
    async Task Up()
    {
      if (DecimalValue >= Data.MaxValue && !IsUnlimitedRange) return;
      await PostValue(DecimalValue + Data.Step);
    }

    public AsyncRelayCommand DownCommand { get; set; }
    async Task Down()
    {
      if (DecimalValue <= Data.MinValue && !IsUnlimitedRange) return;
      await PostValue(DecimalValue - Data.Step);
    }

    #region Apperance
    public bool IsValueMax { get { return (DecimalValue >= Data.MaxValue && !IsUnlimitedRange); } }
    public bool IsValueMin { get { return (DecimalValue <= Data.MinValue && !IsUnlimitedRange); } }
    #endregion
  }
}