using System.Collections.ObjectModel;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Desktop.ViewModels
{
  public class DesktopMainViewModel : MainViewModel
  {
    public DesktopMainViewModel(INavigationService navigationService, AppConfiguration appConfig) : base(navigationService, appConfig)
    {
    }

    public override void OnNavigatedTo()
    {
      base.OnNavigatedTo();

      Widgets = new ObservableCollection<FrameWidget>();
      var f1 = new FrameWidget(Navigation) { Title = "Frame 1" };
      f1.Widgets = new ObservableCollection<WidgetBase>();
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 1", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 2", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 3", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 4", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 5", Size = WidgetSize.Wide });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 6", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 7", Size = WidgetSize.Meduim });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 8", Size = WidgetSize.Wide });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 9", Size = WidgetSize.Large });
      f1.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 10", Size = WidgetSize.Meduim });

      var f2 = new FrameWidget(Navigation) { Title = "Frame 2" };
      f2.Widgets = new ObservableCollection<WidgetBase>();
      f2.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 11", Size = WidgetSize.Meduim });
      f2.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 12", Size = WidgetSize.Meduim });
      f2.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 13", Size = WidgetSize.Wide });
      f2.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 14", Size = WidgetSize.Meduim });
      f2.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 15", Size = WidgetSize.Meduim });

      var f3 = new FrameWidget(Navigation) { Title = "Frame 3" };
      f3.Widgets = new ObservableCollection<WidgetBase>();
      f3.Widgets.Add(new WidgetBase(Navigation) { Title = "Widget 16", Size = WidgetSize.Meduim });

      Widgets.Add(f1); Widgets.Add(f2); Widgets.Add(f3);

    }
  }
}