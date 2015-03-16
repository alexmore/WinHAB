using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class LabelTagTest
  {
    [Test]
    public void Items_IsNotNullAfeterCreate()
    {
      var lt = new LabelTag();
      Assert.That(lt.Items, Is.Not.Null);
    }

    [Test]
    public void Value_ParsedOnSet()
    {
      var lt = new LabelTag();
      lt.Parse("key1:val1,  key2 : val2");
      Assert.That(lt.Items, Is.Not.Empty);
    }

    [TestCase("key1", Result = "val1")]
    [TestCase("key2", Result = "val2")]
    [TestCase("key3", Result = "")]
    public string Parse_IsValid(string key)
    {
      var lt = new LabelTag("key1:val1,     key2   : val2, key3 : ,");
      return lt.Items[key];
    }

    [TestCase("key1", Result = "val1")]
    [TestCase("key2", Result = "val2")]
    [TestCase("key3", Result = "")]
    [TestCase("", Result = null)]
    [TestCase(null, Result = null)]
    public string AccessToItemsGet_IsValid(string key)
    {
      var lt = new LabelTag("key1:val1,     key2   : val2, key3 : ,");
      return lt[key];
    }

    [TestCase("key1", "val1", Result = "val1")]
    [TestCase("", "val1", Result = null)]
    [TestCase(null, "val1", Result = null)]
    public string AccessToItemsSet_IsValid(string key, string value)
    {
      var lt = new LabelTag();
      lt[key] = value;
      return lt[key];
    }
  }
}