using System;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.ViewModels
{
  [TestFixture]
  public class WidgetModelBaseTest
  {
    class TestWidgetModel : WidgetModelBase
    {
      public TestWidgetModel(Widget data) : base(data)
      {
      }
    }

    [Test]
    public void Constructor_NotThrowsException_WhenLinkedPageIsNull()
    {
      Assert.That(() => new TestWidgetModel(new Widget()), Throws.Nothing);

    }

    [Test]
    public void Constructor_NotThrowsException_WhenLinkedPageLinkIsNull()
    {
      Assert.That(() => new TestWidgetModel(new Widget() { LinkedPage = new Page() }), Throws.Nothing);
    }

    [Test]
    public void IsLink_ReturnsTrue_WhenLinkedPageIsNotNull()
    {
      var wm = new TestWidgetModel(new Widget() {LinkedPage = new Page() { Link = new Uri("http://localhost")}});

      Assert.That(wm.IsLink, Is.True);
    }
  }
}