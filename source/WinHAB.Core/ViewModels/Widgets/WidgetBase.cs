using System;
using System.Threading.Tasks;
using WinHAB.Core.Model;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class WidgetBase : ViewModel
  {
    public WidgetBase(INavigationService navigationService, WidgetData data) : base(navigationService)
    {
      Data = data;

      Title = data.Title;
      Value = data.FormattedValue;
      Icon = data.Icon;

      if (data.LinkedPage != null && data.LinkedPage.Link != null)
        LinkedPageUri = data.LinkedPage.Link;
    }

    public WidgetData Data { get; set; }
    public WidgetSize Size { get; set; }

    public virtual Task Initialize()
    {
      return Task.FromResult(0);
    }

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(() => Title); } }

    private string _Value;
    public string Value { get { return _Value; } set { _Value = value; RaisePropertyChanged(() => Value); } }

    private Uri _LinkedPageUri;
    public Uri LinkedPageUri { get { return _LinkedPageUri; } set { _LinkedPageUri = value; RaisePropertyChanged(() => LinkedPageUri); RaisePropertyChanged(()=>IsLink); } }

    private string _icon;
    public string Icon { get { return _icon; } set { _icon = value; RaisePropertyChanged(() => Icon); } }

    public bool IsLink { get { return LinkedPageUri != null; }}
  }
}