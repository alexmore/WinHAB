using Moq;
using NUnit.Framework;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Tests.Core.Fx
{
  [TestFixture]
  public class MvvmExtensionsTest
  {
    [SetUp]
    public void SetupTest()
    {
    }

    [Test]
    public void CleanupView_DoesNotThrow_WhenViewIsNull()
    {
      IView view = null;
      Assert.That(view.CleanupView, Throws.Nothing);
    }

    [Test]
    public void CleanupView_DoesNotThrow_WhenDataContexIsNull()
    {
      var view = Mock.Of<IView>();
      view.DataContext = null;
      Assert.That(view.CleanupView, Throws.Nothing);
    }

    [Test]
    public void CleanupView_DoesNotThrow_WhenDataContexIsNotIViewModel()
    {
      var view = Mock.Of<IView>();
      view.DataContext = "Not IViewModel";
      Assert.That(view.CleanupView, Throws.Nothing);
    }

    [Test]
    public void CleanupView_InvokesIViewModelCleanup_WhenDataContextIsViewModel()
    {
      var vmMock = new Mock<IViewModel>();
      var view = Mock.Of<IView>();
      view.DataContext = vmMock.Object;

      view.CleanupView();

      vmMock.Verify(x=>x.Cleanup());
    }
  }
}