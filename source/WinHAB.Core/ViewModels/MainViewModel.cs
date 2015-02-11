using System.Collections.ObjectModel;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class MainViewModel : ViewModel
  {
    public MainViewModel(INavigationService navigationService, AppConfiguration appConfig) : base(navigationService)
    {
      Title = "Demo House";
      AppConfiguration = appConfig;
    }

    public AppConfiguration AppConfiguration { get; private set; }
    public INavigationService NavigationService { get { return Navigation; }}

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(() => Title); } }

    private ObservableCollection<FrameWidget> _Widgets;
    public ObservableCollection<FrameWidget> Widgets { get { return _Widgets; } set { _Widgets = value; RaisePropertyChanged(() => Widgets); } }

    public override void OnNavigatedTo()
    {
      Widgets = new ObservableCollection<FrameWidget>();
      var f1 = new FrameWidget(Navigation) { Title = "Frame 1"};
      f1.Widgets = new ObservableCollection<WidgetBase>();
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 1", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 2", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 3", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 4", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WeatherWidget(Navigation) { Title = "Widget 5", Size = WidgetSize.Wide });
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 6", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 7", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WeatherWidget(Navigation) { Title = "Widget 8", Size = WidgetSize.Wide });
      f1.Widgets.Add(new PictureWidget(Navigation) { Title = "Widget 9", Size = WidgetSize.Large });
      f1.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 10", Size = WidgetSize.Meduim });

      var f2 = new FrameWidget(Navigation) { Title = "Frame 2" };
      f2.Widgets = new ObservableCollection<WidgetBase>();
      f2.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 11", Size = WidgetSize.Meduim });
      f2.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 12", Size = WidgetSize.Meduim });
      f2.Widgets.Add(new WeatherWidget(Navigation) { Title = "Widget 13", Size = WidgetSize.Wide });
      f2.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 14", Size = WidgetSize.Meduim });
      f2.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 15", Size = WidgetSize.Meduim });

      var f3 = new FrameWidget(Navigation) { Title = "Frame 3" };
      f3.Widgets = new ObservableCollection<WidgetBase>();
      f3.Widgets.Add(new TextWidget(Navigation) { Title = "Widget 16", Size = WidgetSize.Meduim });
      
      Widgets.Add(f1); Widgets.Add(f2); Widgets.Add(f3);
    }
  }
}