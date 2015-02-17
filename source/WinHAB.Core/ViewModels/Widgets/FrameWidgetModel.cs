using System.Collections.ObjectModel;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class FrameWidgetModel : WidgetModelBase
  {
    public FrameWidgetModel(Widget data) : base(data)
    {
      Widgets = new ObservableCollection<WidgetModelBase>();
    }

    private ObservableCollection<WidgetModelBase> _Widgets;
    public ObservableCollection<WidgetModelBase> Widgets { get { return _Widgets; } set { _Widgets = value; RaisePropertyChanged(() => Widgets); } }

    public override void Cleanup()
    {
      base.Cleanup();
      if (Widgets != null)
        foreach (var widget in Widgets)
          widget.Cleanup();
    }
  }
}