using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.ViewModels
{
  [TestFixture]
  public class WidgetModelBaseTest
  {
    class TestWidgetModel : WidgetModelBase
    {
      public TestWidgetModel(Widget data, IRestClientFactory clientFactory) : base(data, clientFactory)
      {
      }

      public Task<bool> CallPollItem(Uri link)
      {
        return PollItem(link);
      }

      public bool IsOnItemChangedCalled = false;
      protected override void OnItemChanged(Item e)
      {
        base.OnItemChanged(e);
        IsOnItemChangedCalled = true;
      }
    }

    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    [Test]
    public void Constructor_ThrowsArgumentException_WhenDataIsNull()
    {
      Assert.That(()=>new TestWidgetModel(null, _vmHelper.ClientFactory), Throws.ArgumentException);
    }

    [Test]
    public void Constructor_NotThrowsArgumentException_WhenDataIsNull()
    {
      Assert.That(() => new TestWidgetModel(new Widget(), _vmHelper.ClientFactory), Throws.Nothing);
    }

    [Test]
    public void Constructor_NotThrowsException_WhenLinkedPageIsNull()
    {
      Assert.That(() => new TestWidgetModel(new Widget(), _vmHelper.ClientFactory), Throws.Nothing);

    }

    [Test]
    public void Constructor_NotThrowsException_WhenLinkedPageLinkIsNull()
    {
      Assert.That(() => new TestWidgetModel(new Widget() { LinkedPage = new Page() }, _vmHelper.ClientFactory), Throws.Nothing);
    }

    [Test]
    public void IsLink_ReturnsTrue_WhenLinkedPageIsNotNull()
    {
      var wm = new TestWidgetModel(new Widget() { LinkedPage = new Page() { Link = new Uri("http://localhost") } }, _vmHelper.ClientFactory);

      Assert.That(wm.IsLink, Is.True);
    }

    [Test]
    public async Task SubscribeItemChanged_RaiseItemChangedEvent()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetLongPollingAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
        .Returns(JsonResources.SwitchItem.AsAsyncResponse);
      var w = new TestWidgetModel(new Widget()
      {
        Item = new Item()
        {
          Link = new Uri("http://some/")
        }
      }, _vmHelper.ClientFactory);

      await w.CallPollItem(new Uri("http://some"));

      Assert.That(w.IsOnItemChangedCalled, Is.True);
    }

    [Test]
    public async Task SetItemState_CallsPostCommandWithStateContent()
    {
      var w = new TestWidgetModel(new Widget()
      {
        Item = new Item()
        {
          Link = new Uri("http://some/")
        }
      }, _vmHelper.ClientFactory);
      
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      await w.SetItemState("ON");

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))
          ));
    }
  }
}