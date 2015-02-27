using System.Collections.ObjectModel;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Tests.Core.ViewModels.Pages
{
  [TestFixture]
  public class MainPageModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    MainPageModel CreateMainPageModel()
    {
      return new MainPageModel(_vmHelper.Navigation, _vmHelper.ClientFactory, _vmHelper.WidgetFactory);
    }

    [Test]
    public void Widgets_CleanupPreviousWidgets_OnNewCollestionAssigned()
    {
      var mp = CreateMainPageModel();
      var vm1Mock = new Mock<FrameWidgetModel>(new Widget());
      var vm2Mock = new Mock<FrameWidgetModel>(new Widget());
      mp.Widgets = new ObservableCollection<FrameWidgetModel>() {vm1Mock.Object, vm2Mock.Object};

      mp.Widgets = null;

      vm1Mock.Verify(x => x.Cleanup(), Times.Once);
      vm2Mock.Verify(x => x.Cleanup(), Times.Once);
    }
  }
}