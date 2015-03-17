using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class WidgetTest
  {
    #region Test Fixture Setup

    [TestFixtureSetUp]
    public void SetupTestFixture()
    {
    }

    [TestFixtureTearDown]
    public void CleanupTestFixture()
    {
    }

    #endregion

    #region Test Setup

    [SetUp]
    public void SetupTest()
    {
    }

    [TearDown]
    public void CleanupTest()
    {
    }

    #endregion

    [TestCase("chart", Result = WidgetType.Chart)]
    [TestCase("colorpicker", Result = WidgetType.Colorpicker)]
    [TestCase("frame", Result = WidgetType.Frame)]
    [TestCase("group", Result = WidgetType.Group)]
    [TestCase("image", Result = WidgetType.Image)]
    [TestCase("selection", Result = WidgetType.Selection)]
    [TestCase("setpoint", Result = WidgetType.Setpoint)]
    [TestCase("slider", Result = WidgetType.Slider)]
    [TestCase("switch", Result = WidgetType.Switch)]
    [TestCase("text", Result = WidgetType.Text)]
    [TestCase("video", Result = WidgetType.Video)]
    [TestCase("webview", Result = WidgetType.Webview)]
    public WidgetType GetWidgetType_ReturnsWidgetTypeEnumValue_OnStringValueOfWidgetType(string widgetType)
    {
      return Widget.GetWidgetType(widgetType);
    }

    [TestCase("Title", Result = "Title")]
    [TestCase("Title[Value]", Result = "Title")]
    [TestCase("[WrongValue]Title [Value]", Result = "Title")]
    [TestCase("Title  [[Value ]", Result = "Title")]
    [TestCase("{Properties}Title [Value]", Result = "Title")]
    [TestCase("Title{Properties} [Value]", Result = "Title")]
    [TestCase("Title [Value]", Result = "Title")]
    [TestCase("[Value]", Result = null)]
    [TestCase("{Properties}", Result = null)]
    [TestCase(null, Result = null)]
    public string TitleProperty_ReturnsValidValue_(string label)
    {
      return new Widget() {Label = label}.Title;
    }

    [TestCase("Title", Result = null)]
    [TestCase("Title[Value]", Result = "Value")]
    [TestCase("Title  [[Value ]", Result = "[Value")]
    [TestCase("{Properties}Title [Value]", Result = "Value")]
    [TestCase("Title{Properties} [Value]", Result = "Value")]
    [TestCase("Title [Value]", Result = "Value")]
    [TestCase("[[Value]]", Result = "[Value")]
    [TestCase("{Properties}", Result = null)]
    [TestCase(null, Result = null)]
    public string ValueProperty_ReturnsValidValue(string label)
    {
      return new Widget() { Label = label }.Value;
    }

    [TestCase("Title", Result = null)]
    [TestCase("Title[Value]", Result = null)]
    [TestCase("[WrongValue]Title [Value]", Result = null)]
    [TestCase("Title  [[Value ]", Result = null)]
    [TestCase("{Properties}Title [Value]", Result = "Properties")]
    [TestCase("Title{Properties} [Value]", Result = "Properties")]
    [TestCase("Title [Value]", Result = null)]
    [TestCase("[Value]", Result = null)]
    [TestCase("{Properties}", Result = "Properties")]
    [TestCase("{{Properties}", Result = "{Properties")]
    [TestCase("{{Properties}}", Result = "{Properties")]
    [TestCase(null, Result = null)]
    public string PropertiesProperty_ReturnsValidValue(string label)
    {

      return new Widget() {Label = label}.Properties.Value;
    }

    [Test]
    public void DeserializingFromJson_IconSetsToNull_WhenIconStringEqualsToNone()
    {
      var w = JsonConvert.DeserializeObject<Widget>(JsonResources.WidgetWithIconNone);

      Assert.That(w.Icon, Is.Null);
    }
  }
}