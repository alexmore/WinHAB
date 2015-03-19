using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Pages;
using WinHAB.Core.ViewModels.Widgets;
using WinHAB.Desktop.Views.Pages;

namespace WinHAB.Tests.Core.ViewModels.Widgets
{
  [TestFixture]
  public class ImageWidgetModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    [Test]
    public void Constructor_SetsWidgetSize_ToLarge()
    {
      var w = new ImageWidgetModel(new Widget(), _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Large));
    }

    [Test]
    public void Constructor_Adds2DropDownsWhenLinkedPageIsNull()
    {
      var w = new ImageWidgetModel(new Widget(), _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);
      Assert.That(w.Dropdowns.Count, Is.EqualTo(2));
      Assert.That(w.Dropdowns[0].Command, Is.EqualTo("FullScreen"));
      Assert.That(w.Dropdowns[1].Command, Is.EqualTo("Refresh"));
    }

    [Test]
    public void Constructor_Adds3DropDownsWhenLinkedPageIsNull()
    {
      var w = new ImageWidgetModel(new Widget() { LinkedPage = new Page()}, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);
      Assert.That(w.Dropdowns.Count, Is.EqualTo(3));
      Assert.That(w.Dropdowns[0].Command, Is.EqualTo("FullScreen"));
      Assert.That(w.Dropdowns[1].Command, Is.EqualTo("Refresh"));
      Assert.That(w.Dropdowns[2].Command, Is.EqualTo("NavigateLinkedPage"));
    }

   
    [Test]
    public void SelectedDropDownItemCommand_WhenFullScreen_NavigatesToImageWidgetPageModel()
    {
      var w = new ImageWidgetModel(new Widget() { LinkedPage = new Page() }, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);

      w.SelectedDropDownItem = new ImageWidgetModel.ImageDropDownItem() {Command = "FullScreen"};
      _vmHelper.NavigationMock.Verify(m=>m.NavigateAsync<ImageWidgetPageModel>(It.Is<ImageWidgetModel>(x=>x == w)) );
    }

    [Test]
    public void SelectedDropDownItemCommand_WhenRefresh_LoadsImage()
    {
      var data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 8 };
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(data.AsAsyncResponse);
      
      var w = new ImageWidgetModel(new Widget() { LinkedPage = new Page(), Url = new Uri("http://some")}, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);

      w.SelectedDropDownItem = new ImageWidgetModel.ImageDropDownItem() { Command = "Refresh" };
      Assert.That(w.ImageStream, Is.Not.Null);
      Assert.That((w.ImageStream as MemoryStream).ToArray(), Is.EquivalentTo(data));
    }

    [Test]
    public void SelectedDropDownItemCommand_WhenLoadLinkedPage_ExecutesNavigateLinkedPageCommand()
    {
      bool isCommandExecuted = false;

      var w = new ImageWidgetModel(new Widget() { LinkedPage = new Page(), Url = new Uri("http://some") }, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);
      w.NavigateLinkedPageCommand = new AsyncRelayCommand<WidgetModelBase>(async x => isCommandExecuted = x == w ? true : false);

      w.SelectedDropDownItem = new ImageWidgetModel.ImageDropDownItem() { Command = "NavigateLinkedPage" };
      Assert.That(isCommandExecuted, Is.True);
    }


    [Test]
    public async Task InitializeAsync_DoesNotThrow_WhenDataLinkIsNull()
    {
      var w = new ImageWidgetModel(new Widget(), _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);
      Assert.That(async ()=> await w.InitializeAsync(null), Throws.Nothing);
    }

    [Test]
    public async Task InitializeAsync_LoadsImage()
    {
      var data = new byte[] {0, 1, 2, 3, 4, 5, 6, 8};
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(data.AsAsyncResponse);

      var w = new ImageWidgetModel(new Widget() { Url = new Uri("http://localhost")}, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);

      await w.InitializeAsync(null);

      Assert.That(w.ImageStream, Is.Not.Null);
      Assert.That((w.ImageStream as MemoryStream).ToArray(), Is.EquivalentTo(data));
    }

    [Test]
    public async Task InitializeAsync_SetsImageStreamToNull_WhenLoadingFails()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var w = new ImageWidgetModel(new Widget() {Url = new Uri("http://localhost")}, _vmHelper.ClientFactory,
      _vmHelper.Navigation, _vmHelper.Timer);

      await w.InitializeAsync(null);

      Assert.That(w.ImageStream, Is.Null);
    }

    [Test]
    public async Task InitializeAsync_DoesNotStartTime_WhenRefreshIsZero()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var w = new ImageWidgetModel(new Widget() { Url = new Uri("http://localhost")}, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);

      await w.InitializeAsync(null);

      _vmHelper.TimerMock.Verify(x=>x.Start(), Times.Never);
    }

    [Test]
    public async Task InitializeAsync_StartsTime_WhenRefreshIsNotZero()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var w = new ImageWidgetModel(new Widget() { Url = new Uri("http://localhost"), Refresh = 10}, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);

      await w.InitializeAsync(null);

      _vmHelper.TimerMock.Verify(x => x.Start(), Times.Once);
    }

    [Test]
    public async Task TimerTick_LoadsImage()
    {
      byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 8 };
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(data.AsAsyncResponse);

      var w = new ImageWidgetModel(new Widget() { Url = new Uri("http://localhost"), Refresh = 10}, _vmHelper.ClientFactory, _vmHelper.Navigation, _vmHelper.Timer);
      
      await w.InitializeAsync(null);

      byte[] timerData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(timerData.AsAsyncResponse);

      _vmHelper.TimerMock.Raise(x=>x.Tick += null, EventArgs.Empty);

      Assert.That(w.ImageStream, Is.Not.Null);
      Assert.That((w.ImageStream as MemoryStream).ToArray(), Is.EquivalentTo(timerData));
    }
  }
}