using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinHAB.Core.Models.Converters;

namespace WinHAB.Core.Models
{
  public class Sitemap
  {
    public string Name { get; set; }
    public string Label { get; set; }
    public Uri Link { get; set; }

    [JsonProperty("homepage")]
    [JsonConverter(typeof(SitemapHomepageLinkJsonConverter))]
    public Uri HomepageLink { get; set; }
  }
}