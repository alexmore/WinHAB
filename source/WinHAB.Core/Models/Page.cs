using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WinHAB.Core.Models.JsonConverters;

namespace WinHAB.Core.Models
{
  public class Page
  {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public Uri Link { get; set; }

    public Page Parent { get; set; }

    public bool Leaf { get; set; }

    [JsonProperty("widget")]
    [JsonConverter(typeof(ToListJsonConverter<Widget>))]
    public List<Widget> Widgets { get; set; }
  }
}