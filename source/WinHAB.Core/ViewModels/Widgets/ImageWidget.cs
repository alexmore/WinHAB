using System;
using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Fx;
using WinHAB.Core.Models;
using WinHAB.Core.Mvvm;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class ImageWidget : WidgetBase
  {
    private readonly OpenHabClient _client;
    private readonly ITimer _timer;

    public ImageWidget(INavigationService navigationService, Widget data, OpenHabClient client, ITimer timer) : base(navigationService, data)
    {
      _client = client;
      _timer = timer;
      Size = WidgetSize.Large;

      ViewImageCommand = new RelayCommand(()=>Navigation.Navigate<ImageWidgetPage>(x=>x.Add("image", this)));
    }

    private byte[] _imageCache = null;

    public Stream ImageStream { get { return _imageCache != null ? new MemoryStream(_imageCache) : null; } }

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
        var stream = await _client.GetStreamAsync(Data.Url);
        using (var memStream = new MemoryStream())
        {
          await stream.CopyToAsync(memStream);
          _imageCache = memStream.ToArray();
        }
        RaisePropertyChanged(() => ImageStream);
      }
      catch (Exception)
      {
        _imageCache = null;
        RaisePropertyChanged(()=>ImageStream);
      }

      Waiter.Hide();
    }

    public RelayCommand ViewImageCommand { get; set; }

    public override void Cleanup()
    {
      _timer.Stop();

      base.Cleanup();
    }
  }
}