using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WinHAB.Core.Model.Converters;

namespace WinHAB.Core.Model
{
  public class PageData
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public Uri Link { get; set; }

    public PageData Parent { get; set; }

    public bool Leaf { get; set; }

    [JsonProperty("widget")]
    [JsonConverter(typeof(ToListJsonConverter<WidgetData>))]
    public List<WidgetData> Widgets { get; set; }
  }
}