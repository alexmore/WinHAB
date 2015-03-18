using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Tests.Core.ViewModels.Widgets
{
  [TestFixture]
  public class TextWidgetModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    [TestCase("", Result = WidgetSize.Meduim)]
    [TestCase("{ Size = Wide }", Result = WidgetSize.Wide)]
    [TestCase("{ Size = Large }", Result = WidgetSize.Large)]
    [TestCase("{ Size = Medium }", Result = WidgetSize.Meduim)]
    [TestCase("{ Size = Wrong }", Result = WidgetSize.Meduim)]
    public WidgetSize Constructor_SetsSize_DependingWidgetProperties(string label)
    {
      var tw = new TextWidgetModel(new Widget() { Label = label}, _vmHelper.ClientFactory);
      return tw.Size;
    }
  }
}