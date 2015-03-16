using System;
using System.Threading;
using System.Threading.Tasks;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class WidgetModelBase : ViewModel
  {
    protected readonly IRestClientFactory ClientFactory;

    protected WidgetModelBase(Widget data, IRestClientFactory clientFactory)
    {
      ClientFactory = clientFactory;
      Data = data;

      Title = data.Title;
      Value = data.Value;
      Icon = data.Icon;

      if (data.LinkedPage != null && data.LinkedPage.Link != null)
        LinkedPage = data.LinkedPage.Link;
    }

    public Widget Data { get; set; }
    public WidgetSize Size { get; set; }

    private string _title;
    public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(() => Title); } }

    private string _value;
    public string Value { get { return _value; } set { _value = value; RaisePropertyChanged(() => Value); } }
    
    private Uri _linkedPage;
    public Uri LinkedPage { get { return _linkedPage; } set { _linkedPage = value; RaisePropertyChanged(() => LinkedPage); RaisePropertyChanged(()=>IsLink); } }

    private string _icon;
    public string Icon { get { return _icon; } set { _icon = value; RaisePropertyChanged(() => Icon); } }

    public bool IsLink { get { return LinkedPage != null; } }

    #region Long-polling

    protected CancellationTokenSource ItemPollingCancellationTokenSource = new CancellationTokenSource();

    private event EventHandler<Item> _itemChanged;
    public event  EventHandler<Item> ItemChanged
    {
      add {
        lock (this)
        {
          if (_itemChanged == null)
          {
            _itemChanged += value;
            StartItemPolling();
          }
          else
            _itemChanged += value;
        }
      }

      remove
      {
        lock (this)
        {
          _itemChanged -= value;

          if (_itemChanged == null)
            StopItemPolling();
        }
      }
    }

    protected virtual void OnItemChanged(Item e)
    {
      EventHandler<Item> handler = _itemChanged;
      if (handler != null) handler(this, e);
    }

    protected void StartItemPolling()
    {
      if (Data == null || Data.Item == null || Data.Item.Link == null)
        throw new ArgumentException("Item.Link property can not be null.");

      Task.Run(async () =>
      {
        while (true)
        {
          if (! await PollItem(Data.Item.Link))
            break;
        }
      }, ItemPollingCancellationTokenSource.Token);
    }

    protected async Task<bool> PollItem(Uri itemLink)
    {
      try
      {
        using (var cln = ClientFactory.Create())
        {
          var i =
            await cln.GetLongPollingAsync(itemLink, ItemPollingCancellationTokenSource.Token).AsItemAsync();

          OnItemChanged(i);

          ItemPollingCancellationTokenSource.Token.ThrowIfCancellationRequested();
          await Task.Delay(800);
          return true;
        }
      }
      catch (OperationCanceledException oce)
      {
        return false;
      }

    }

    public void StopItemPolling()
    {
      ItemPollingCancellationTokenSource.Cancel();
    }
    #endregion

    public override void Cleanup()
    {
      ItemPollingCancellationTokenSource.Cancel();
      
      base.Cleanup();
    }
  }
}