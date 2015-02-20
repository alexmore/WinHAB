using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Tests.Core.Fx.Mvvm
{
  [TestFixture]
  public class NavigationServerBaseTest
  {
    [SetUp]
    public void SetupTest()
    {
      _viewFactoryMock = new Mock<IViewFactory>();
      
      _navMock = new Mock<TestNavigationService>(_viewFactory);
      _navMock.CallBase = true;
    }
    
    public abstract class TestNavigationService : NavigationServiceBase
    {
      protected TestNavigationService(IViewFactory factory) : base(factory)
      {
      }

      public void SetCurrentView(IView view) { CurrentView = view; }
    }

    private Mock<IViewFactory> _viewFactoryMock;
    private IViewFactory _viewFactory { get { return _viewFactoryMock.Object; } }

    private Mock<TestNavigationService> _navMock;
    private TestNavigationService _nav { get { return _navMock.Object; } }

    [Test]
    public async Task Constructor_Creates_HistoryAndGoBackCommand()
    {
      Assert.That(_nav.History, Is.Not.Null); 
      Assert.That(_nav.GoBackCommand, Is.Not.Null);
    }

    [Test]
    public async Task NavigateAsyncGenericNoParams_InvokesNavigateAsyncGenericWithParameterNull()
    {
      _navMock.Setup(x => x.NavigateAsync(It.IsAny<Type>(), null)).Returns(Task.FromResult(Mock.Of<IViewModel>()));
      
      await _nav.NavigateAsync<IViewModel>();

      _navMock.Verify(x => x.NavigateAsync<IViewModel>(It.Is<object>(v => v == null)));
    }

    [Test]
    public async Task NavigateAsyncGenericWithParameter_InvokesNavigateAsync()
    {
      _navMock.Setup(x => x.NavigateAsync(It.IsAny<Type>(), It.IsAny<string>())).Returns(Task.FromResult(Mock.Of<IViewModel>()));
      

      await _nav.NavigateAsync<IViewModel>("some parameter");

      _navMock.Verify(x => x.NavigateAsync(It.IsAny<Type>(), It.IsAny<string>()));
    }

    [Test]
    public void NavigateAsync_ThrowsArgumentException_WhenIViewFactoryCreateReturnsNull()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(()=> null);

      Assert.That(async () =>  await _nav.NavigateAsync(typeof (IViewModel), null), Throws.ArgumentException);
    }

    [Test]
    public void NavigateAsync_ThrowsArgumentException_WhenIViewDataContextIsNull()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(() => { var m = Mock.Of<IView>(); m.DataContext = null; return m; });

      Assert.That(async () => await _nav.NavigateAsync(typeof(IViewModel), null), Throws.ArgumentException);
    }

    [Test]
    public void NavigateAsync_ThrowsArgumentException_WhenIViewDataContextIsNotIViewModel()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(() => { var m = Mock.Of<IView>(); m.DataContext = "Not ViewModel"; return m; });

      Assert.That(async () => await _nav.NavigateAsync(typeof(IViewModel), null), Throws.ArgumentException);
    }

    [Test]
    public async Task NavigateAsync_InvokesNavigateView_WhenAllParametersIsValid()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(() => { var m = Mock.Of<IView>(); m.DataContext = Mock.Of<IViewModel>(); return m; });

      await _nav.NavigateAsync(typeof (IViewModel), null);

      _navMock.Verify(x=>x.NavigateView(It.IsAny<IView>()));
    }

    [Test]
    public async Task NavigateAsync_SetsCurrentVaiew_WhenNavigateViewInvoked()
    {
      var viewMock = Mock.Of<IView>(); 
      viewMock.DataContext = Mock.Of<IViewModel>();
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(viewMock);

      await _nav.NavigateAsync(typeof(IViewModel), null);

      Assert.That(_nav.CurrentView, Is.SameAs(viewMock));
    }

    [Test]
    public async Task NavigateAsync_InvokesIViewModelInitialize_WithParameter()
    {
      var vmMock = new Mock<IViewModel>();
      var viewMock = Mock.Of<IView>();
      viewMock.DataContext = vmMock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(viewMock);

      await _nav.NavigateAsync(typeof(IViewModel), "vmParameter");

      vmMock.Verify(x => x.InitializeAsync(It.Is<string>(p => p == "vmParameter")));
    }

    [Test]
    public async Task NavigateAsync_AddsCurrentViewToHistory_WhenNavigatedForSecondViewModel()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(() => { var m = Mock.Of<IView>(); m.DataContext = Mock.Of<IViewModel>(); return m; });

      await _nav.NavigateAsync(typeof(IViewModel), null);
      var currentView = _nav.CurrentView;
      await _nav.NavigateAsync(typeof(IViewModel), null);

      Assert.That(_nav.History.Count, Is.EqualTo(1));
      Assert.That(_nav.History.Peek(), Is.SameAs(currentView));
      Assert.That(_nav.CurrentView, Is.Not.SameAs(currentView));
    }

    [Test]
    public async Task CanGoBack_ReturnsFalsee_WhenHistoryIsEmpty()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(() => { var m = Mock.Of<IView>(); m.DataContext = Mock.Of<IViewModel>(); return m; });

      Assert.That(() => _nav.CanGoBack(), Is.False);
    }

    [Test]
    public async Task CanGoBack_ReturnsTrue_WhenHistoryIsNotEmpty()
    {
      _viewFactoryMock.Setup(x => x.Create(It.IsAny<Type>())).Returns(() => { var m = Mock.Of<IView>(); m.DataContext = Mock.Of<IViewModel>(); return m; });
      
      _nav.History.Push(Mock.Of<IView>());

      Assert.That(()=>_nav.CanGoBack(), Is.True);
    }

    [Test]
    public async Task GoBack_CleanupView_WhenCurrentViewIsNotNull()
    {
      var vm1Mock = new Mock<IViewModel>();
      var view1 = Mock.Of<IView>();
      view1.DataContext = vm1Mock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.Is<Type>(t => t == vm1Mock.Object.GetType()))).Returns(view1);

      var vm2Mock = new Mock<IViewModel>();
      var view2 = Mock.Of<IView>();
      view2.DataContext = vm2Mock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.Is<Type>(t => t == vm2Mock.Object.GetType()))).Returns(view2);
      
      _nav.History.Push(view1);
      _nav.SetCurrentView(view2);

      _nav.GoBack();

      vm2Mock.Verify(x=>x.Cleanup());
    }

    [Test]
    public async Task GoBack_PopsView_FromHistory()
    {
      var vm1Mock = new Mock<IViewModel>();
      var view1 = Mock.Of<IView>();
      view1.DataContext = vm1Mock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.Is<Type>(t => t == vm1Mock.Object.GetType()))).Returns(view1);

      var vm2Mock = new Mock<IViewModel>();
      var view2 = Mock.Of<IView>();
      view2.DataContext = vm2Mock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.Is<Type>(t => t == vm2Mock.Object.GetType()))).Returns(view2);

      _nav.History.Push(view1);

      Assert.That(_nav.History.Count, Is.EqualTo(1));

      _nav.GoBack();

      Assert.That(_nav.History.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GoBack_SetsCurrentViewAndNvigateIt()
    {
      var vm1Mock = new Mock<IViewModel>();
      var view1 = Mock.Of<IView>();
      view1.DataContext = vm1Mock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.Is<Type>(t => t == vm1Mock.Object.GetType()))).Returns(view1);

      var vm2Mock = new Mock<IViewModel>();
      var view2 = Mock.Of<IView>();
      view2.DataContext = vm2Mock.Object;
      _viewFactoryMock.Setup(x => x.Create(It.Is<Type>(t => t == vm2Mock.Object.GetType()))).Returns(view2);

      _nav.History.Push(view1);
      _nav.SetCurrentView(view2);
      
      _nav.GoBack();

      Assert.That(_nav.CurrentView, Is.SameAs(view1));
      _navMock.Verify(x=>x.NavigateView(It.Is<IView>(v=>v == view1)));
    }

    [Test]
    public void GoBackCommand_InvokesGoBackMethod()
    {
      _nav.GoBackCommand.Execute(null);
      _navMock.Verify(x=>x.GoBack());
    }


  }
}