using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WinHAB.Core.Model.Converters;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.Model
{
  public class WidgetData
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
    [JsonConverter(typeof(ToListJsonConverter<MappingData>))]
    public List<MappingData> Mappings;
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

     public ItemData Item { get; set; }
    public PageData LinkedPage { get; set; }

    [JsonProperty("widget")]
    [JsonConverter(typeof(ToListJsonConverter<WidgetData>))]
    public List<WidgetData> Widgets { get; set; }

    #region Calculated properties
    public string Title
    {
      get
      {
        if (string.IsNullOrWhiteSpace(Label)) return null;
        if (Label.IndexOf('[') == -1) return Label;
        return Label.Substring(0, Label.LastIndexOf('[')).Trim();
      }
    }

    public string FormattedValue
    {
      get
      {
        if (string.IsNullOrWhiteSpace(Label)) return null;
        const string pattern = @"\[(.*?)\]";
        var match = Regex.Match(Label, pattern);
        return match.Groups[1].Value.Trim();
      }
    }
    #endregion

    #region Helpers

    public static WidgetType GetWidgetType(string typeString)
    {
      switch (typeString)
      {
        case "Chart": return WidgetType.Chart;
        case "Colorpicker": return WidgetType.Colorpicker;
        case "Frame": return WidgetType.Frame;
        case "Group": return WidgetType.Group;
        case "Image": return WidgetType.Image;
        case "Selection": return WidgetType.Selection;
        case "Setpoint": return WidgetType.Setpoint;
        case "Slider": return WidgetType.Slider;
        case "Switch": return WidgetType.Switch;
        case "Text": return WidgetType.Text;
        case "Video": return WidgetType.Video;
        case "Webview": return WidgetType.Webview;
        
        default:
          return WidgetType.Unknown;
      }
    }
    #endregion
  }
}