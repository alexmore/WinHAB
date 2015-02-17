using System;
using System.Threading.Tasks;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.ViewModels
{
  public class WidgetBase : ViewModel
  {
    protected WidgetBase(Widget data)
    {
      Data = data;

      Title = data.Title;
      Value = data.FormattedValue;
      Icon = data.Icon;

      if (data.LinkedPage != null && data.LinkedPage.Link != null)
        LinkedPageUri = data.LinkedPage.Link;
    }

    public Widget Data { get; set; }
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