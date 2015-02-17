using WinHAB.Core.Models;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels.Widgets
{
  public class TextWidget : WidgetBase
  {
    public TextWidget(INavigationService navigationService, WidgetData data) : base(navigationService, data)
    {
      Size = WidgetSize.Meduim;
      
      if (!string.IsNullOrWhiteSpace(data.FormattedValue))
      Size = data.FormattedValue.Length > 31 ? WidgetSize.Large : data.FormattedValue.Length > 15 ? WidgetSize.Wide : WidgetSize.Meduim; 
    }
  }
}