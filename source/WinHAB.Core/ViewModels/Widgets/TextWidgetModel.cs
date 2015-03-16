using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class TextWidgetModel : WidgetModelBase
  {
    public TextWidgetModel(Widget data, IRestClientFactory clientFactory) : base(data, clientFactory)
    {
      Size = WidgetSize.Meduim;
      
      if (!string.IsNullOrWhiteSpace(data.FormattedValue))
      Size = data.FormattedValue.Length > 31 ? WidgetSize.Large : data.FormattedValue.Length > 15 ? WidgetSize.Wide : WidgetSize.Meduim; 
    }
  }
}