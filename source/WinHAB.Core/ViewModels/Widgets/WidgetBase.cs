using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class WidgetBase : ViewModel
  {
    public WidgetBase(INavigationService navigationService) : base(navigationService)
    {
    }

    public WidgetSize Size { get; set; }

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(() => Title); } }

    private string _Value;
    public string Value { get { return _Value; } set { _Value = value; RaisePropertyChanged(() => Value); } }

  }
}