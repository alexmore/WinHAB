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
    [Test]
    public void Cleanup_InvokesCleanup_ForEveryItemInWidgetsCollection()
    {
      var f = new FrameWidgetModel(new Widget());

      var wMock1 = new Mock<WidgetModelBase>(new Widget());
      var wMock2 = new Mock<WidgetModelBase>(new Widget());

      f.Widgets.Add(wMock1.Object);
      f.Widgets.Add(wMock2.Object);

      f.Cleanup();

      wMock1.Verify(x=>x.Cleanup());
      wMock2.Verify(x=>x.Cleanup());
    }
  }
}