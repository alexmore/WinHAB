using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
  public class SelectionWidgetModel : WidgetModelBase
  {
    private readonly INavigationService _navigation;

    public SelectionWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
      : base(data, clientFactory)
    {
      if (data == null || data.Mappings == null || data.Mappings.Count < 2)
        throw new ArgumentException(
          "SelectionWidgetModel can not be created when Data is null, Mappings is null or contains less than two items.");

      _navigation = navigation;

      Size = WidgetSize.Meduim;
      
      if (Data != null && Data.Properties != null)
        Size = Data.Properties.GetSize() ?? WidgetSize.Meduim;

      Mappings = new ObservableCollection<Mapping>(data.Mappings);

      UpdateValue(Data.Item);
      
      ItemChanged += (sender, item) =>
      {
        HideProgressIndicator();
        Data.Item = item;
        UpdateValue(Data.Item);
      };
    }

    private ObservableCollection<Mapping> _mappings;
    public ObservableCollection<Mapping> Mappings
    {
      get { return _mappings; }
      set { _mappings = value; RaisePropertyChanged(() => Mappings); }
    }

    private bool _PostSelectionOnUpdate = true;

    private Mapping _selectedMapping;
    public Mapping SelectedMapping
    {
      get { return _selectedMapping; }
      set
      {
        _selectedMapping = value;
        RaisePropertyChanged(() => SelectedMapping);
        if (_PostSelectionOnUpdate)
          PostSelection(value);
      }
    }
    
    private bool _isOffState;
    public bool IsOffState
    {
      get { return _isOffState; }
      set { _isOffState = value; RaisePropertyChanged(()=>IsOffState); }
    }

    void UpdateValue(Item item)
    {
      string state = null;
      if (item != null)
        state = item.State;

      _PostSelectionOnUpdate = false;
      SelectedMapping = Mappings.FirstOrDefault(x => x.Command == state);
      _PostSelectionOnUpdate = true;
      if (SelectedMapping != null)
      {
        Value = SelectedMapping.Label;
        IsOffState = SelectedMapping.Properties.Values.IsOff;
      }
      else
      {
        Value = null;
        IsOffState = false;
      }
    }

    async void PostSelection(Mapping mapping)
    {
      if (mapping == null) return;

      ShowProgressIndicator();
      try
      {
        await SetItemState(mapping.Command);
      }
      catch (Exception e)
      {
        HideProgressIndicator();
        _navigation.ShowMessage(Localization.Strings.MessageExceptionOnWidgetSetItemStateTitle,
          Localization.Strings.MessageExceptionOnWidgetSetItemState + ":\r\n" + e.Message, () => { });
      }
    }
  }
}