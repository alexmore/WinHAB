using System.Collections.Generic;
using Newtonsoft.Json;
using WinHAB.Core.Models.JsonConverters;

namespace WinHAB.Core.Models
{
  public class SitemapList
  {
    [JsonProperty("sitemap")]
    [JsonConverter(typeof(ToListJsonConverter<Sitemap>))]
    public List<Sitemap> Sitemaps { get; set; }
  }
}