using Newtonsoft.Json;
using WinHAB.Core.Fx;
using WinHAB.Core.Models.Converters;

namespace WinHAB.Core.Models
{
  public class Mapping
  {
    public Mapping()
    {
      Properties = new WidgetProperties();
    }

    public string Command { get; set; }

    private string _label;
    public string Label
    {
      get { return _label; }
      set
      {
        var wl = WidgetLabel.Parse(value);
        _label = wl.Title;
        Properties = wl.Properties;
      }
    }
    
    public WidgetProperties Properties { get; set; }
  }
}