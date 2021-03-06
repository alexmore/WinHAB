﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Models.JsonConverters
{
  public class WidgetTypeJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(Widget));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      JToken token = JToken.Load(reader);

      if (token == null) return WidgetType.Unknown;

      var typeString = token.ToObject<string>();

      return Widget.GetWidgetType(typeString);
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