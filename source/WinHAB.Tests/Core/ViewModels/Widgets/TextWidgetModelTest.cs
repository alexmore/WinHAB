using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Tests.Core.ViewModels.Widgets
{
  [TestFixture]
  public class TextWidgetModelTest
  {
    [TestCase(null, Result = WidgetSize.Meduim)] // Null
    [TestCase("", Result = WidgetSize.Meduim)]
    // > 15 chars - Wide
    [TestCase("[1234567890123456]", Result = WidgetSize.Wide)]
    // > 31 chars - Large
    [TestCase("[12345678901234567890123456789012]", Result = WidgetSize.Large)]
    public WidgetSize Constructor_SetsWidgetSize_DependingOnValueLength(string label)
    {
      var tw = new TextWidgetModel(new Widget() {Label = label});
      return tw.Size;
    }
  }
}