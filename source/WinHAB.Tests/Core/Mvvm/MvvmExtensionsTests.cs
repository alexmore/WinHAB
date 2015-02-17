using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Tests.Core.Mvvm
{
  [TestClass]
  public class MvvmExtensionsTests
  {
    [TestMethod]
    public void MvvmExtensionsViewCleanupTest()
    {

      var viewMock = new Mock<IView>();
      viewMock.SetupAllProperties();
      
      var viewModelMock = new Mock<IViewModel>();
      viewModelMock.Setup(x => x.Cleanup()).Verifiable();

      viewMock.Object.DataContext = viewModelMock.Object;

      viewMock.Object.Cleanup();

      viewModelMock.Verify(x=>x.Cleanup());

      viewMock.Object.DataContext = null;
      viewMock.Object.Cleanup();
      
      (null as IView).Cleanup();
    }
  }
}
