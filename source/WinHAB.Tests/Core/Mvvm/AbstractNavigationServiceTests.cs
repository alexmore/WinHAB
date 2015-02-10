using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using WinHAB.Core.Mvvm;
using WinHAB.Desktop;

namespace WinHAB.Tests.Core.Mvvm
{
  /// <summary>
  /// Summary description for DesktopNavigationService
  /// </summary>
  [TestClass]
  public class AbstractNavigationServiceTests
  {
    private TestEnvironment _env = new TestEnvironment();

    [TestInitialize]
    public void PrepareTest()
    {
      _env.Init();
    }

    class TestNavigationService : AbstractNavigationService
    {
      public TestNavigationService(IViewModelViewFactory factory) : base(factory)
      {
      }

      public int HistoryCount { get { return History.Count; }}
      public Action OnNavigatedToView;

      public Stack<IView> HistoryView {
        get { return History; }
      }

      public void CleanCurrentView()
      {
        CurrentView = null;
      }

      public override void NavigateView(IView view)
      {
        CurrentView = view;
        if (OnNavigatedToView != null) OnNavigatedToView();
      }

      public override Task ShowMessageAsync(string title, string text)
      {
        throw new NotImplementedException();
      }

      public override void ShowMessage(string title, string text, Action onClose)
      {
        throw new NotImplementedException();
      }
    }

    [TestMethod]
    public void AbstractNavigationServiceNavigateViewModelTest()
    {
      var viewMock = new Mock<IView>();
      viewMock.SetupAllProperties();
      ConstructorParameters ctorParameters = null;
      
      var viewModelMock = new Mock<IViewModel>();
      viewModelMock.Setup(x => x.OnNavigatedTo()).Verifiable();

      _env.ViewModelViewFactory.Map(viewModelMock.Object.GetType(), viewMock.Object.GetType());
      _env.ViewModelViewFactoryMock.Setup(x => x.CreateView(It.IsAny<Type>())).Returns(() => viewMock.Object);
      _env.ViewModelViewFactoryMock.Setup(x => x.CreateViewModel(It.IsAny<Type>(), It.IsAny<Action<ConstructorParameters>>()))
        .Returns(() => viewModelMock.Object)
        .Callback<Type, Action<ConstructorParameters>>((t,a) => ctorParameters = a.GetConstructorParameters());

      var nav = new TestNavigationService(_env.ViewModelViewFactory);

      bool isNavigatedToView = false;
      nav.OnNavigatedToView = () => isNavigatedToView = true;
      
      // IViewModel
      nav.Navigate(viewModelMock.Object);
      Assert.AreSame(viewModelMock.Object, nav.CurrentView.DataContext);
      viewModelMock.Verify(x=>x.OnNavigatedTo());
      Assert.IsTrue(isNavigatedToView);

      // Navigate(Type, ctorParams)
      nav.ClearHistory();
      ctorParameters = null;
      Assert.AreSame(viewModelMock.Object, nav.Navigate(viewModelMock.Object.GetType(), x=>x.Add("a","b")));
      Assert.AreSame(viewMock.Object, nav.CurrentView);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);
      Assert.IsNotNull(ctorParameters);
      Assert.AreEqual("ab", ctorParameters.Parameters.First().Key + ctorParameters.Parameters.First().Value.ToString());

      // Navigate(Type)
      nav.ClearHistory();
      Assert.AreSame(viewModelMock.Object, nav.Navigate(viewModelMock.Object.GetType()));
      Assert.AreSame(viewMock.Object, nav.CurrentView);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // Navigate<>()
      nav.ClearHistory();
      Assert.AreSame(viewModelMock.Object, nav.Navigate<IViewModel>());
      Assert.AreSame(viewMock.Object, nav.CurrentView);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);

      // Navigate<T>(ctorParams)
      nav.ClearHistory();
      ctorParameters = null;
      Assert.AreSame(viewModelMock.Object, nav.Navigate<IViewModel>(x=>x.Add("a","b")));
      Assert.AreSame(viewMock.Object, nav.CurrentView);
      Assert.AreSame(viewMock.Object.DataContext, viewModelMock.Object);
      Assert.IsNotNull(ctorParameters);
      Assert.AreEqual("ab", ctorParameters.Parameters.First().Key + ctorParameters.Parameters.First().Value.ToString());
    }

    [TestMethod]
    public void AbstractNavigationServiceHistoryTest()
    {
      var viewMock = new Mock<IView>();
      viewMock.SetupAllProperties();
      
      var viewModelMock = new Mock<IViewModel>();
      viewModelMock.Setup(x => x.Cleanup()).Verifiable();

      _env.ViewModelViewFactory.Map(viewModelMock.Object.GetType(), viewMock.Object.GetType());
      _env.ViewModelViewFactoryMock.Setup(x => x.CreateView(It.IsAny<Type>())).Returns(() => viewMock.Object);
      _env.ViewModelViewFactoryMock.Setup(x => x.CreateViewModel(It.IsAny<Type>(), It.IsAny<Action<ConstructorParameters>>()))
        .Returns(() => viewModelMock.Object);

      var nav = new TestNavigationService(_env.ViewModelViewFactory);
      
      Assert.IsFalse(nav.CanGoBack());

      nav.Navigate(viewModelMock.Object);
      Assert.IsFalse(nav.HistoryView.Count > 0);
      nav.Navigate(viewModelMock.Object);
      Assert.IsTrue(nav.HistoryView.Count > 0);
      Assert.IsTrue(nav.CanGoBack());

      nav.GoBack();
      Assert.AreEqual(0, nav.HistoryCount);
      viewModelMock.Verify(x=>x.Cleanup(), Times.Once);

      nav.GoBack();
      nav.Navigate(viewModelMock.Object);
      nav.Navigate(viewModelMock.Object);

      nav.CleanCurrentView();
      viewModelMock.ResetCalls();
      nav.ClearHistory();
      Assert.AreEqual(0, nav.HistoryCount);
      Assert.IsNull(nav.CurrentView);
      viewModelMock.Verify(x => x.Cleanup(), Times.Exactly(2));
      
    }
  }
}
