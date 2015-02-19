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
    public WidgetType GetWidgetType_OnStringValueOfWidgetType_ReturnsWidgetTypeEnumValue(string widgetType)
    {
      return Widget.GetWidgetType(widgetType);
    }

    [TestCase("Title 1", Result = "Title 1")]
    [TestCase("Title 2 [12]", Result = "Title 2")]
    [TestCase("Title 4 [[3]", Result = "Title 4")]
    public string TitleProperty_OnLabelWithTittleAndValue_ReturnsParsedTitle(string label)
    {
      return new Widget() {Label = label}.Title;
    }

    [TestCase("Title 1", Result = null)]
    [TestCase("Title 2 [12]", Result = "12")]
    [TestCase("[1]Title 3 [1]", Result = "1")]
    [TestCase("Title 4 [[3 ]", Result = "[3")]
    public string FormattedValue_OnLabelWithTitleAndValue_ReturnsParsedValue(string label)
    {
      return new Widget() { Label = label }.FormattedValue;
    }
  }
}