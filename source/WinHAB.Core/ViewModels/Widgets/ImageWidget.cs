using System;
using System.IO;
using System.Threading.Tasks;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class ImageWidget : WidgetBase
  {
    private readonly OpenHabClient _client;
    private readonly ITimer _timer;

    public ImageWidget(INavigationService navigationService, WidgetData data, OpenHabClient client, ITimer timer) : base(navigationService, data)
    {
      _client = client;
      _timer = timer;
      Size = WidgetSize.Large;
    }

    private Stream _ImageStream;
    public Stream ImageStream { get { return _ImageStream; } set { _ImageStream = value; RaisePropertyChanged(() => ImageStream); } }
    
    public override async Task Initialize()
    {
      await LoadImageAsync(Data.Url);

      if (Data.Refresh > 0)
      {
        _timer.Interval = TimeSpan.FromSeconds(Data.Refresh);
        _timer.Start();
        _timer.Tick += async (sender, args) => await LoadImageAsync(Data.Url);
      }
    }

    async Task LoadImageAsync(Uri url)
    {
      Waiter.Show();
      if (url == null) return;

      try
      {
        ImageStream = await _client.GetStreamAsync(Data.Url);
      }
      catch (Exception)
      {
        ImageStream = null;
      }

      Waiter.Hide();
    }

    public override void Cleanup()
    {
      _timer.Stop();

      base.Cleanup();
    }
  }
}