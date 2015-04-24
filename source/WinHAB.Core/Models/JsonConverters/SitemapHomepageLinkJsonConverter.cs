using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Models.JsonConverters
{
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