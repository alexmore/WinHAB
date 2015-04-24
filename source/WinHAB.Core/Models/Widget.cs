using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WinHAB.Core.Models.JsonConverters;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.Models
{
  public class Widget
  {
    public Widget()
    {
      LabelParts = new WidgetLabel();
    }

    [JsonProperty("widgetId")]
    public string Id { get; set; }
    [JsonConverter(typeof(WidgetTypeJsonConverter))]
    public WidgetType Type { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(IconStringJsonConverter))]
    public string Icon { get; set; }
    public string LabelColor { get; set; }
    public string ValueColor { get; set; }
    
    [JsonProperty("mapping")]
    [JsonConverter(typeof(ToListJsonConverter<Mapping>))]
    public List<Mapping> Mappings;
    public bool SwitchSupport { get; set; }
    public int SendFrequency { get; set; }
    public string Separator { get; set; }
    public int Refresh { get; set; }
    public int Height { get; set; }
    public decimal MinValue { get; set; }
    public decimal MaxValue { get; set; }
    public decimal Step { get; set; }
    public Uri Url { get; set; }
    public string Encoding { get; set; }
    public string Service { get; set; }
    public string Period { get; set; }

     public Item Item { get; set; }
    public Page LinkedPage { get; set; }

    [JsonProperty("widget")]
    [JsonConverter(typeof(ToListJsonConverter<Widget>))]
    public List<Widget> Widgets { get; set; }

    #region Label

    private string _label;
    public string Label
    {
      get { return _label; }
      set
      {
        _label = value;
        LabelParts = WidgetLabel.Parse(value);
      }
    }

    public WidgetLabel LabelParts { get; set; }

    public string Title { get { return LabelParts.Title; } }
    public string Value { get { return LabelParts.Value; } }
    public WidgetProperties Properties { get { return LabelParts.Properties; }}

    #endregion

    #region Helpers

    public static WidgetType GetWidgetType(string typeString)
    {
      switch (typeString.ToLower())
      {
        case "chart": return WidgetType.Chart;
        case "colorpicker": return WidgetType.Colorpicker;
        case "frame": return WidgetType.Frame;
        case "group": return WidgetType.Group;
        case "image": return WidgetType.Image;
        case "selection": return WidgetType.Selection;
        case "setpoint": return WidgetType.Setpoint;
        case "slider": return WidgetType.Slider;
        case "switch": return WidgetType.Switch;
        case "text": return WidgetType.Text;
        case "video": return WidgetType.Video;
        case "webview": return WidgetType.Webview;
        
        default:
          return WidgetType.Unknown;
      }
    }
    #endregion
  }
}