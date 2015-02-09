using System;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WinHAB.Core.Mvvm;
using WinHAB.Desktop;

namespace WinHAB.Tests.Desktop
{
  /// <summary>
  /// Summary description for DesktopNavigationService
  /// </summary>
  [TestClass]
  public class DesktopNavigationServiceTests
  {
    private TestEnvironment _env = new TestEnvironment();

    [TestInitialize]
    public void PrepareTest()
    {
      _env.Init();
    }
    
    [TestMethod]
    public void DesktopNavigationServiceNavigateViewInstanceTest()
    {
      Mock<IView> viewMock = new Mock<IView>();
      var nav = new DesktopNavigationService(_env.NavigationHost, _env.ViewModelViewFactory);
      nav.NavigateView(viewMock.Object);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content);
    }
  }
}
