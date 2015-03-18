using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class MappingTest
  {
    [TestCase("Label", Result = "Label")]
    [TestCase("Label {Property}", Result = "Label")]
    public string ParsesLabel_WhenLabelValueSets(string label)
    {
      var m = new Mapping() {Label = label};
      return m.Label;
    }

    [TestCase("Label", Result = null)]
    [TestCase("Label {Property}", Result = "")]
    public string ParsesProperty_WhenLabelValueSets(string label)
    {
      var m = new Mapping() { Label = label };
      return m.Properties["Property"];
    }
    
    [Test]
    public void PropertiesProperty_IsAlwaysIsNotNull()
    {
      var m = new Mapping();
      Assert.That(m.Properties, Is.Not.Null);
    }
  }
}