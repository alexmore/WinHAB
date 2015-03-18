using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class WidgetPropertiesTest
  {
    [Test]
    public void Items_IsNotNullAfeterCreate()
    {
      var lt = new WidgetProperties();
      Assert.That(lt.Items, Is.Not.Null);
    }

    [Test]
    public void Value_ParsedOnSet()
    {
      var lt = new WidgetProperties();
      lt.Parse("key1:val1,  key2 : val2");
      Assert.That(lt.Items, Is.Not.Empty);
    }

    [TestCase("key1", Result = "val1")]
    [TestCase("key2", Result = "val2")]
    [TestCase("key3", Result = "")]
    [TestCase("singlekey", Result = "")]
    [TestCase("key5", Result = "val5 = val")]
    public string Parse_IsValid(string key)
    {
      var lt = new WidgetProperties("key1=val1,     key2   = val2, SingleKey,  key3 = , , key5= val5 = val");
      return lt.Items[key];
    }

    [TestCase("key1", Result = "val1")]
    [TestCase("key2", Result = "val2")]
    [TestCase("key3", Result = "")]
    [TestCase("SingleKey", Result = "")]
    [TestCase("", Result = null)]
    [TestCase(null, Result = null)]
    public string AccessToItemsGet_IsValid(string key)
    {
      var lt = new WidgetProperties("key1=val1,     key2   = val2, SingleKey, key3 = ,");
      return lt[key];
    }

    [TestCase("key1", "val1", Result = "val1")]
    [TestCase("", "val1", Result = null)]
    [TestCase(null, "val1", Result = null)]
    public string AccessToItemsSet_IsValid(string key, string value)
    {
      var lt = new WidgetProperties();
      lt[key] = value;
      return lt[key];
    }

    [Test]
    public void GetValueNotCaseSensitiveKey()
    {
      var wp = new WidgetProperties();

      wp["Key1"] = "Key1Value";
      
      Assert.That(wp["kEy1"], Is.EqualTo("Key1Value"));
    }

    [Test]
    public void SetValueNotCaseSensitiveKey()
    {
      var wp = new WidgetProperties();

      wp["Key1"] = "Key1Value";
      wp["kEy1"] = "NewKey1Value";
      Assert.That(wp["Key1"], Is.EqualTo("NewKey1Value"));
    }
  }
}