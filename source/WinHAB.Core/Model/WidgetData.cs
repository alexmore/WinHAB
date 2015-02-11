using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WinHAB.Core.Model
{
  public class WidgetData
  {
    [JsonProperty("widgetId")]
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }

    public string Label { get; set; }
    public string Icon { get; set; }
    public string LabelColor { get; set; }
    public string ValueColor { get; set; }

    // TODO: What is Mapping in Widget?
	  // widget-specific attributes
	  // @XmlElement(name="mapping")
	  // public List<MappingBean> mappings = new ArrayList<MappingBean>();
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
  }
}