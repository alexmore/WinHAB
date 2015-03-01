using System.Linq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.Models
{
  [TestFixture]
  public class ModelExtensionTest
  {
    [Test]
    public void CreateFrameWrappedCollection_ReturnsNull_WhenInputCollectionIsNull()
    {
      Widget[] widgets = null;
      
      var res = widgets.CreateFrameWrappedCollection();
      Assert.That(res, Is.Null);
    }

    [Test]
    public void CreateFrameWrappedCollection_ReturnsEmptyCollection_WhenInputCollectionIsEmpty()
    {
      Widget[] widgets = new Widget[0];

      var res = widgets.CreateFrameWrappedCollection();
      Assert.That(res, Is.Not.Null);
      Assert.That(res, Is.Empty);
    }

    [Test]
    public void CreateFrameWrappedCollection_ReturnsUnwrappedFrame()
    {
      Widget[] widgets = new Widget[] { new Widget() { Type = WidgetType.Frame, Label = "Unwrapped frame" }};

      var res = widgets.CreateFrameWrappedCollection();
      
      Assert.That(res.Count(), Is.EqualTo(1));
      Assert.That(res.First().Type, Is.EqualTo(WidgetType.Frame));
      Assert.That(res.First().Label, Is.EqualTo("Unwrapped frame"));
    }

    [Test]
    public void CreateFrameWrappedCollection_WrapsNonFrameWidgetInFrame()
    {
      Widget[] widgets = new Widget[]
      {
        new Widget() { Type = WidgetType.Frame, Label = "Frame1" },
        new Widget() { Type = WidgetType.Chart, Label = "Chart1" }
      };

      var res = widgets.CreateFrameWrappedCollection();

      Assert.That(res.Count(), Is.EqualTo(2));
      Assert.That(res.First().Type, Is.EqualTo(WidgetType.Frame));
      Assert.That(res.Last().Type, Is.EqualTo(WidgetType.Frame));
    }

    [Test]
    public void CreateFrameWrappedCollection_WrapsOrderNonFrameWidgetInFrame()
    {
      Widget[] widgets = new Widget[]
      {
        new Widget() {Type = WidgetType.Frame, Label = "Frame1"},
        new Widget() {Type = WidgetType.Chart, Label = "Chart1"},
        new Widget() {Type = WidgetType.Frame, Label = "Frame2"},
        new Widget() {Type = WidgetType.Chart, Label = "Chart2"}
      };

      var res = widgets.CreateFrameWrappedCollection();

      Assert.That(res.Count(), Is.EqualTo(4));
      Assert.That(res.Select(x => x.Type), Has.All.EqualTo(WidgetType.Frame));

      Assert.That(res[1].Widgets[0].Label, Is.EqualTo("Chart1"));
      Assert.That(res[1].Label, Is.Null);
      
      Assert.That(res[3].Widgets[0].Label, Is.EqualTo("Chart2"));
      Assert.That(res[3].Label, Is.Null);
    }
  }
}