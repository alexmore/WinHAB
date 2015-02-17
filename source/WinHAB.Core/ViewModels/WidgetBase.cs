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
        LinkedPage = data.LinkedPage.Link;
    }

    public Widget Data { get; set; }
    public WidgetSize Size { get; set; }

    private string _title;
    public string Title { get { return _title; } set { _title = value; RaisePropertyChanged(() => Title); } }

    private string _value;
    public string Value { get { return _value; } set { _value = value; RaisePropertyChanged(() => Value); } }

    private Uri _linkedPage;
    public Uri LinkedPage { get { return _linkedPage; } set { _linkedPage = value; RaisePropertyChanged(() => LinkedPage); RaisePropertyChanged(()=>IsLink); } }

    private string _icon;
    public string Icon { get { return _icon; } set { _icon = value; RaisePropertyChanged(() => Icon); } }

    public bool IsLink { get { return LinkedPage != null; }}
  }
}