using Moq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Tests.Core.ViewModels.Widgets
{
  [TestFixture]
  public class FrameWidgetModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    [Test]
    public void Cleanup_InvokesCleanup_ForEveryItemInWidgetsCollection()
    {
      var f = new FrameWidgetModel(new Widget(), _vmHelper.ClientFactory);

      var wMock1 = new Mock<WidgetModelBase>(new Widget(), _vmHelper.ClientFactory);
      var wMock2 = new Mock<WidgetModelBase>(new Widget(), _vmHelper.ClientFactory);

      f.Widgets.Add(wMock1.Object);
      f.Widgets.Add(wMock2.Object);

      f.Cleanup();

      wMock1.Verify(x=>x.Cleanup());
      wMock2.Verify(x=>x.Cleanup());
    }
  }
}