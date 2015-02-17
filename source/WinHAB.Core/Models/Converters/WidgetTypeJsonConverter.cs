using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Core.Models.Converters
{
  public class WidgetTypeJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(WidgetData));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      JToken token = JToken.Load(reader);

      if (token == null) return WidgetType.Unknown;

      var typeString = token.ToObject<string>();

      return WidgetData.GetWidgetType(typeString);
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