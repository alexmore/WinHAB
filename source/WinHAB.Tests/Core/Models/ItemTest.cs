using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class ItemTest
  {
    [TestCase("somewrong", Result = ItemType.Unknown)]
    [TestCase("GroupItem", Result = ItemType.Group)]
    [TestCase("SwitchItem", Result = ItemType.Switch)]
    [TestCase("NumberItem", Result = ItemType.Number)]
    [TestCase("ColorItem", Result = ItemType.Color)]
    [TestCase("ContactItem", Result = ItemType.Contact)]
    [TestCase("DateTimeItem", Result = ItemType.DateTime)]
    [TestCase("DimmerItem", Result = ItemType.Dimmer)]
    [TestCase("RollershutterItem", Result = ItemType.Rollershutter)]
    [TestCase("StringItem", Result = ItemType.String)]
    public ItemType GetItemType_ReturnsItemTypeEnumValue_OnStringValueOfItmeType(string itemType)
    {
      return Item.GetItemType(itemType);
    }
  }
}