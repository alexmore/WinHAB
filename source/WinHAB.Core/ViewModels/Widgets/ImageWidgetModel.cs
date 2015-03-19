using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class ImageWidgetModel : WidgetModelBase, INavigationWidget
  {
    private readonly INavigationService _navigationService;
    private readonly ITimer _timer;
    
    public ImageWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigationService, ITimer timer) : base(data, clientFactory)
    {
      _navigationService = navigationService;
      _timer = timer;
      Size = WidgetSize.Large;
      
      Dropdowns = new ObservableCollection<ImageDropDownItem>();
      Dropdowns.Add(new ImageDropDownItem()
      {
        Title = Localization.Strings.ButtonViewImage,
        Icon = "FullScreenIcon",
        Command = "FullScreen"
      });

      Dropdowns.Add(new ImageDropDownItem()
      {
        Title = Localization.Strings.ButtonRefresh,
        Icon = "ReloadIcon",
        Command = "Refresh"
      });

      if (Data.LinkedPage != null)
      Dropdowns.Add(new ImageDropDownItem()
      {
        Title = Data.Title,
        Icon = "ArrowRightIcon",
        Command = "NavigateLinkedPage"
      });
    }

    private byte[] _imageCache = null;
    public Stream ImageStream { get { return _imageCache != null ? new MemoryStream(_imageCache) : null; } }

    public override async Task InitializeAsync(object parameter)
    {
      await LoadImageAsync(Data.Url);

      if (Data.Refresh > 0)
      {
        _timer.Interval = TimeSpan.FromMilliseconds(Data.Refresh);
        _timer.Start();
        _timer.Tick += async (sender, args) => await LoadImageAsync(Data.Url);
      }
    }

    async Task LoadImageAsync(Uri url)
    {
      if (url == null) return;

      ShowProgressIndicator();
      try
      {
        using (var cln = ClientFactory.Create())
        {
          var stream = await cln.GetAsync(url).AsStreamAsync();
          
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

      HideProgressIndicator();
    }

    public AsyncRelayCommand ViewImageCommand { get; set; }

    #region Dropdown

    public class ImageDropDownItem
    {
      public string Title { get; set; }
      public string Icon { get; set; }
      public string Command { get; set; }
    }

    private ObservableCollection<ImageDropDownItem> _dropdowns;
    public ObservableCollection<ImageDropDownItem> Dropdowns
    {
      get { return _dropdowns; }
      set { _dropdowns = value; RaisePropertyChanged(()=>Dropdowns); }
    }

    public ImageDropDownItem SelectedDropDownItem
    {
      get { return null; }
      set
      {
        OnSelectedDropDownItemChangedAsync(value);
      }
    }

    protected async Task OnSelectedDropDownItemChangedAsync(ImageDropDownItem item)
    {
      if (item != null)
      {
        switch (item.Command)
        {
          case "FullScreen" :
            await _navigationService.NavigateAsync<ImageWidgetPageModel>(this);
            break;
          case "Refresh" :
            await LoadImageAsync(Data.Url);
            break;
          case "NavigateLinkedPage":
            if (NavigateLinkedPageCommand != null) await NavigateLinkedPageCommand.ExecuteAsync(this);
            break;
        }
      }
    }
    #endregion

    public override void Cleanup()
    {
      _timer.Stop();

      base.Cleanup();
    }

    public AsyncRelayCommand<WidgetModelBase> NavigateLinkedPageCommand { get; set; }
  }
}