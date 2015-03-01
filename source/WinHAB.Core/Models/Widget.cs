using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WinHAB.Core.Models.Converters;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.Models
{
  public class Widget
  {
    [JsonProperty("widgetId")]
    public string Id { get; set; }
    [JsonConverter(typeof(WidgetTypeJsonConverter))]
    public WidgetType Type { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
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

    #region Calculated properties

    Tuple<string, string> ParseLabel(string labelValue)
    {
      if (labelValue == null) return new Tuple<string, string>(null, null);
      
      var match = Regex.Match(labelValue, @"\[(.*?)\]");

      var resTitle = labelValue;
      string resValue = null;

      if (match.Success)
      {
        var g = match.Groups[1];
        if (g.Success)
        {
          resValue = g.Value.Trim();
          resTitle = (labelValue.Substring(0, g.Index-1) + labelValue.Substring(g.Index+1 + g.Length)).Trim();
        }
      }

      return new Tuple<string, string>(resTitle.Trim(), resValue);
    }

    public string Title
    {
      get { return ParseLabel(Label).Item1; }
    }

    public string FormattedValue
    {
      get { return ParseLabel(Label).Item2; }
    }
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