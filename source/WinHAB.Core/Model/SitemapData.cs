using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinHAB.Core.Model.Converters;

namespace WinHAB.Core.Model
{
  public class SitemapData
  {
    public string Name { get; set; }
    public string Label { get; set; }
    public Uri Link { get; set; }

    [JsonProperty("homepage")]
    [JsonConverter(typeof(SitemapHomepageLinkJsonConverter))]
    public Uri HomepageLink { get; set; }
  }

  public class SitemapListData
  {
    [JsonProperty("sitemap")]
    [JsonConverter(typeof(ToListJsonConverter<SitemapData>))]
    public List<SitemapData> Sitemaps { get; set; }
  }

  public class SitemapHomepageLinkJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(Uri));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var token = JToken.Load(reader);

      if (token == null) return null;

      if (token["link"] == null) return null;
      
      return new Uri(token["link"].ToString());
    }

    public override bool CanWrite
    {
      get { return false; }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}