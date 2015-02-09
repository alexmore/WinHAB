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
      nav.Navigate(viewMock.Object);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object, nav.CurrentVeiw);

      nav.Navigate(new Mock<IView>().Object);
      Assert.IsTrue(nav.CanGoBack());
    }

    [TestMethod]
    public void DesktopNavigationServiceHistoryTest()
    {
      var viewMock1 = new Mock<IView>();
      var viewMock2 = new Mock<IView>();
      var viewMock3 = new Mock<IView>();
      bool isView3Cleaned = false;
      viewMock3.SetupAllProperties();
      var vm = new Mock<IViewModel>();
      vm.Setup(x => x.Cleanup()).Callback(() => isView3Cleaned = true);
      viewMock3.Object.DataContext = vm.Object;


      var nav = new DesktopNavigationService(_env.NavigationHost, _env.ViewModelViewFactory);
      nav.Navigate(viewMock1.Object);
      Assert.AreSame(viewMock1.Object, nav.CurrentVeiw);

      nav.Navigate(viewMock2.Object);
      Assert.AreSame(viewMock2.Object, nav.CurrentVeiw);
      Assert.IsTrue(nav.CanGoBack());

      nav.Navigate(viewMock3.Object);
      Assert.AreSame(viewMock3.Object, nav.CurrentVeiw);
      Assert.IsTrue(nav.CanGoBack());

      nav.GoBack();
      Assert.IsTrue(isView3Cleaned);
      Assert.AreSame(viewMock2.Object, nav.CurrentVeiw);
      Assert.IsTrue(nav.CanGoBack());

      nav.ClearHistory();
      Assert.IsFalse(nav.CanGoBack());
      Assert.IsNull(nav.CurrentVeiw);

      try
      {
        nav.GoBack();
        Assert.IsTrue(false);
      }
      catch
      {
        Assert.IsTrue(true);
      }
    }

    [TestMethod]
    public void DesktopNavigationServiceNavigateViewModelInstanceTest()
    {
      var viewMock = new Mock<IView>();
      viewMock.SetupAllProperties();
      var viewModelMock = new Mock<IViewModel>();

      _env.ViewModelViewFactory.Map(viewModelMock.Object.GetType(), viewMock.Object.GetType());
      _env.ViewModelViewFactoryMock.Setup(x => x.CreateView(It.IsAny<Type>())).Returns(() => viewMock.Object);
      _env.ViewModelViewFactoryMock.Setup(x => x.CreateViewModel(It.IsAny<Type>(), It.IsAny<Action<ConstructorParameters>>()))
        .Returns(() => viewModelMock.Object);

      var nav = new DesktopNavigationService(_env.NavigationHost, _env.ViewModelViewFactory);
      
      // Navigate(IViewModel)
      nav.Navigate(viewModelMock.Object);
      Assert.AreSame(viewMock.Object, nav.CurrentVeiw);
      Assert.IsNotNull(_env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content); 
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // Navigate(Type, ctorParams)
      nav.ClearHistory();
      Assert.AreSame(viewModelMock.Object, nav.Navigate(viewModelMock.Object.GetType(), null));
      Assert.AreSame(viewMock.Object, nav.CurrentVeiw);
      Assert.IsNotNull(_env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // Navigate(Type)
      nav.ClearHistory();
      Assert.AreSame(viewModelMock.Object, nav.Navigate(viewModelMock.Object.GetType()));
      Assert.AreSame(viewMock.Object, nav.CurrentVeiw);
      Assert.IsNotNull(_env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // Navigate<>()
      nav.ClearHistory();
      Assert.AreSame(viewModelMock.Object, nav.Navigate<IViewModel>());
      Assert.AreSame(viewMock.Object, nav.CurrentVeiw);
      Assert.IsNotNull(_env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // Navigate<T>(ctorParams)
      nav.ClearHistory();
      Assert.AreSame(viewModelMock.Object, nav.Navigate<IViewModel>(null));
      Assert.AreSame(viewMock.Object, nav.CurrentVeiw);
      Assert.IsNotNull(_env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object, _env.NavigationHost.Content);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // OnNavigatedTo calling test
      bool isOnNavigateToCalled = false;
      viewModelMock.Setup(x => x.OnNavigatedTo()).Callback(() => isOnNavigateToCalled = true);
      nav.Navigate(viewModelMock.Object);
      Assert.IsTrue(isOnNavigateToCalled);
    }
  }
}
