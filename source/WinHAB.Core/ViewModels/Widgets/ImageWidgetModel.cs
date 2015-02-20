using System;
using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class ImageWidgetModel : WidgetModelBase
  {
    private readonly INavigationService _navigationService;
    private readonly IRestClientFactory _clientFactory;
    private readonly ITimer _timer;

    public ImageWidgetModel(INavigationService navigationService, Widget data, IRestClientFactory clientFactory, ITimer timer) : base(data)
    {
      _navigationService = navigationService;
      _clientFactory = clientFactory;
      _timer = timer;
      Size = WidgetSize.Large;

      ViewImageCommand = new AsyncRelayCommand(async () => await _navigationService.NavigateAsync<ImageWidgetPageModel>(this));
    }

    private byte[] _imageCache = null;

    public Stream ImageStream { get { return _imageCache != null ? new MemoryStream(_imageCache) : null; } }

    public override async Task InitializeAsync(object parameter)
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
      TaskProgress.Show();
      if (url == null) return;

      try
      {
        using (var cln = _clientFactory.Create())
        {
          var stream = await cln.GetAsync(Data.Url).AsStreamAsync();
          
          using (var memStream = new MemoryStream())
          {
            await stream.CopyToAsync(memStream);
            _imageCache = memStream.ToArray();
          }
          
          RaisePropertyChanged(() => ImageStream);
        }
      }
      catch (Exception)
      {
        _imageCache = null;
        RaisePropertyChanged(()=>ImageStream);
      }

      TaskProgress.Hide();
    }

    public AsyncRelayCommand ViewImageCommand { get; set; }

    public override void Cleanup()
    {
      _timer.Stop();

      base.Cleanup();
    }
  }
}