using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WinHAB.Core.Mvvm;

namespace WinHAB.Tests.Core.Mvvm
{
  [TestClass]
  public class MvvmExtensionsTests
  {
    [TestMethod]
    public void MvvmExtensionsViewCleanupTest()
    {
      bool isCleanupCalled = false;

      var viewMock = new Mock<IView>();
      viewMock.SetupAllProperties();
      
      var viewModelMock = new Mock<IViewModel>();
      viewModelMock.Setup(x => x.Cleanup()).Callback(() => isCleanupCalled = true);

      viewMock.Object.DataContext = viewModelMock.Object;

      viewMock.Object.Cleanup();

      Assert.IsTrue(isCleanupCalled);

      viewMock.Object.DataContext = null;
      viewMock.Object.Cleanup();
      
      (null as IView).Cleanup();
    }
  }
}
