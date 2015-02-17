using System.Collections.ObjectModel;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class FrameWidget : WidgetBase
  {
    public FrameWidget(Widget data) : base(data)
    {
      Widgets = new ObservableCollection<WidgetBase>();
    }

    private ObservableCollection<WidgetBase> _Widgets;
    public ObservableCollection<WidgetBase> Widgets { get { return _Widgets; } set { _Widgets = value; RaisePropertyChanged(() => Widgets); } }

    public override void Cleanup()
    {
      base.Cleanup();
      if (Widgets != null)
        foreach (var widget in Widgets)
          widget.Cleanup();
    }
  }
}