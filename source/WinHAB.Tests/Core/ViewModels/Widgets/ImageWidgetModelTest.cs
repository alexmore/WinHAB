using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

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

    private ImageWidgetModel CreateImageWidgetModel(Widget data)
    {
      return new ImageWidgetModel(_vmHelper.Navigation, data, _vmHelper.ClientFactory, _vmHelper.Timer);
    }

    [Test]
    public void Constructor_SetsWidgetSize_ToLarge()
    {
      var iw = new ImageWidgetModel(_vmHelper.Navigation, new Widget(), _vmHelper.ClientFactory, _vmHelper.Timer);

      Assert.That(iw.Size, Is.EqualTo(WidgetSize.Large));
    }

    [Test]
    public void Constructor_InitsViewImageCommand()
    {
      var iw = new ImageWidgetModel(_vmHelper.Navigation, new Widget(), _vmHelper.ClientFactory, _vmHelper.Timer);

      Assert.That(iw.ViewImageCommand, Is.Not.Null);
    }

    [Test]
    public async Task InitializeAsync_DoesNotThrow_WhenDataLinkIsNull()
    {
      var iw = CreateImageWidgetModel(new Widget());
      
      Assert.That(async ()=> await iw.InitializeAsync(null), Throws.Nothing);
    }

    [Test]
    public async Task InitializeAsync_HidesTaskProgress_WhenDataLinkIsNull()
    {
      var iw = CreateImageWidgetModel(new Widget());

      await iw.InitializeAsync(null);

      Assert.That(iw.TaskProgress.IsVisible, Is.False);
    }

    [Test]
    public async Task InitializeAsync_LoadsImage()
    {
      byte[] data = new byte[] {0, 1, 2, 3, 4, 5, 6, 8};
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(data.AsAsyncResponse);

      var iw = CreateImageWidgetModel(new Widget() {Url = new Uri("http://localhost")});

      await iw.InitializeAsync(null);

      Assert.That(iw.ImageStream, Is.Not.Null);
      Assert.That((iw.ImageStream as MemoryStream).ToArray(), Is.EquivalentTo(data));
      Assert.That(iw.IsImageLoadingFailed, Is.False);
    }

    [Test]
    public async Task InitializeAsync_SetsImageStreamToNull_WhenLoadingFails()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var iw = CreateImageWidgetModel(new Widget() { Url = new Uri("http://localhost") });

      await iw.InitializeAsync(null);

     Assert.That(iw.ImageStream, Is.Null);
    }

    [Test]
    public async Task InitializeAsync_SetsImageLoadingFailsLabel_WhenLoadingFails()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var iw = CreateImageWidgetModel(new Widget() { Url = new Uri("http://localhost") });

      await iw.InitializeAsync(null);

      Assert.That(iw.IsImageLoadingFailed, Is.True);
    }

    [Test]
    public async Task InitializeAsync_DoesNotStartTime_WhenRefreshIsZero()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var iw = CreateImageWidgetModel(new Widget() { Url = new Uri("http://localhost") });

      await iw.InitializeAsync(null);

      _vmHelper.TimerMock.Verify(x=>x.Start(), Times.Never);
    }

    [Test]
    public async Task InitializeAsync_StartsTime_WhenRefreshIsNotZero()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Throws<Exception>();

      var iw = CreateImageWidgetModel(new Widget() { Url = new Uri("http://localhost"), Refresh = 10 });

      await iw.InitializeAsync(null);

      _vmHelper.TimerMock.Verify(x => x.Start(), Times.Once);
    }

    [Test]
    public async Task TimerTick_LoadsImage()
    {
      byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 8 };
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(data.AsAsyncResponse);

      var iw = CreateImageWidgetModel(new Widget() { Url = new Uri("http://localhost"), Refresh = 10});
      
      await iw.InitializeAsync(null);

      byte[] timerData = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns(timerData.AsAsyncResponse);

      _vmHelper.TimerMock.Raise(x=>x.Tick += null, EventArgs.Empty);

      Assert.That(iw.ImageStream, Is.Not.Null);
      Assert.That((iw.ImageStream as MemoryStream).ToArray(), Is.EquivalentTo(timerData));
    }

  }
}