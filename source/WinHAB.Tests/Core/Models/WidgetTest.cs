using System.Linq;
using System.Text.RegularExpressions;
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
    [TestCase("{Tag}Title [Value]", Result = "Title")]
    [TestCase("Title{Tag} [Value]", Result = "Title")]
    [TestCase("Title [Value]", Result = "Title")]
    [TestCase("[Value]", Result = null)]
    [TestCase("{Tag}", Result = null)]
    [TestCase(null, Result = null)]
    public string TitleProperty_ReturnsValidValue_(string label)
    {
      return new Widget() {Label = label}.Title;
    }

    [TestCase("Title", Result = null)]
    [TestCase("Title[Value]", Result = "Value")]
    [TestCase("Title  [[Value ]", Result = "[Value")]
    [TestCase("{Tag}Title [Value]", Result = "Value")]
    [TestCase("Title{Tag} [Value]", Result = "Value")]
    [TestCase("Title [Value]", Result = "Value")]
    [TestCase("[[Value]]", Result = "[Value")]
    [TestCase("{Tag}", Result = null)]
    [TestCase(null, Result = null)]
    public string ValueProperty_ReturnsValidValue(string label)
    {
      return new Widget() { Label = label }.Value;
    }

    [TestCase("Title", Result = null)]
    [TestCase("Title[Value]", Result = null)]
    [TestCase("[WrongValue]Title [Value]", Result = null)]
    [TestCase("Title  [[Value ]", Result = null)]
    [TestCase("{Tag}Title [Value]", Result = "Tag")]
    [TestCase("Title{Tag} [Value]", Result = "Tag")]
    [TestCase("Title [Value]", Result = null)]
    [TestCase("[Value]", Result = null)]
    [TestCase("{Tag}", Result = "Tag")]
    [TestCase("{{Tag}", Result = "{Tag")]
    [TestCase("{{Tag}}", Result = "{Tag")]
    [TestCase(null, Result = null)]
    public string TagProperty_ReturnsValidValue(string label)
    {
      return new Widget() { Label = label }.Tag;
    }
  }
}