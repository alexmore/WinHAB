using System;
using Newtonsoft.Json;
using WinHAB.Core.Models.JsonConverters;

namespace WinHAB.Core.Models
{
  public class Item
  {
    [JsonConverter(typeof(ItemtTypeJsonConverter))]
    public ItemType Type { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public Uri Link { get; set; }

    public static ItemType GetItemType(string typeString)
    {
      switch (typeString.ToLower())
      {
        case "groupitem":
          return ItemType.Group;
        case "switchitem":
          return ItemType.Switch;
        case "numberitem":
          return ItemType.Number;
        case "coloritem":
          return ItemType.Color;
        case "contactitem":
          return ItemType.Contact;
        case "datetimeitem":
          return ItemType.DateTime;
        case "dimmeritem":
          return ItemType.Dimmer;
        case "rollershutteritem":
          return ItemType.Rollershutter;
        case "stringitem":
          return ItemType.String;

        default:
          return ItemType.Unknown;
      }
    }
  }
}