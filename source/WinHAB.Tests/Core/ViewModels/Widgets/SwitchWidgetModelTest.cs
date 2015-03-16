using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Tests.Core.ViewModels.Widgets
{
  [TestFixture]
  public class SwitchWidgetModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    [Test]
    public void Constructor_SetsSize_ToMedium()
    {
      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { Link = new Uri("http://some") } }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Meduim));
    }

    [Test]
    public void Constructor_SetsIcon_ToDataValue()
    {
      var w = new SwitchWidgetModel(new Widget() { Icon = "someIcon", Item = new Item() { Link = new Uri("http://some") } }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Icon, Is.EqualTo("someIcon"));
    }

    [Test]
    public void Constructor_Creates_PostCommand()
    {
      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { Link = new Uri("http://some") } }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.PostCommand, Is.Not.Null);
    }
    
    [Test]
    public void Constructor_SetsStateActive_WhenWidgetItemHasONState()
    {
      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { State = "ON", Link = new Uri("http://some")}}, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.State, Is.EqualTo(SwitchWidgetState.Active));
    }

    [Test]
    public void Constructor_SetsStateInactive_WhenWidgetItemHasOFFState()
    {
      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { State = "OFF", Link = new Uri("http://some")} }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.State, Is.EqualTo(SwitchWidgetState.Inactive));
    }

    [Test]
    public void Constructor_SetsStateNormal_WhenWidgetHasSingleMappingValue()
    {
      var w = new SwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "OFF" }, 
        Mappings = new List<Mapping>(new [] { new Mapping() { Command = "1", Label = "Turn on" }})
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.State, Is.EqualTo(SwitchWidgetState.Normal));
    }

    [Test]
    public void Constructor_ThrowsArgumentException_WhenMappingsContainsMoreThan1Mappings()
    {
      Assert.Throws<ArgumentException>(() => new SwitchWidgetModel(new Widget()
      {
        Item = new Item() {State = "OFF"},
        Mappings =
          new List<Mapping>(new[]
          {new Mapping() {Command = "1", Label = "Turn on"}, new Mapping() {Command = "1", Label = "Turn on"}})
      }, _vmHelper.ClientFactory, _vmHelper.Navigation)
        );

    }

    [Test]
    public void Constructor_SetsIsButtonToTrue_WhenWidgetSingleMappingValue()
    {
      var w = new SwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "OFF" },
        Mappings = new List<Mapping>(new[] { new Mapping() { Command = "1", Label = "Turn on" } })
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsButton, Is.True);
    }

    [Test]
    public async Task PostCommand_PostsOFF_WhenStateIsON()
    {
      string command = string.Empty;
      _vmHelper.RestClientMock.Setup(
        x => x.PostAsync(It.Is<Uri>(u => u == new Uri(@"http:\\link")), It.IsAny<StringContent>()))
        .Callback(async (Uri u, HttpContent s) => command = await (s as StringContent).ReadAsStringAsync())
        .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted))); ;
      
      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { State = "ON", Link = new Uri(@"http://link/")} }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      await w.PostCommand.ExecuteAsync(null);
      Assert.That(command, Is.EqualTo("OFF"));
    }

    [Test]
    public async Task PostCommand_PostsON_WhenStateIsOFF()
    {
      string command = string.Empty;
      _vmHelper.RestClientMock.Setup(
        x => x.PostAsync(It.Is<Uri>(u => u == new Uri(@"http:\\link")), It.IsAny<StringContent>()))
        .Callback(async (Uri u, HttpContent s) => command = await (s as StringContent).ReadAsStringAsync())
        .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted))); ;

      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { State = "OFF", Link = new Uri(@"http://link") } }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      await w.PostCommand.ExecuteAsync(null);
      Assert.That(command, Is.EqualTo("ON"));
    }

    [Test]
    public async Task PostCommand_PostsMappingCommand_WhenWidgetSingleMappingValue()
    {
      string command = string.Empty;
      _vmHelper.RestClientMock.Setup(
        x => x.PostAsync(It.Is<Uri>(u => u == new Uri(@"http://link")), It.IsAny<StringContent>()))
        .Callback(async (Uri u, HttpContent s) => command = await (s as StringContent).ReadAsStringAsync())
        .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted))); ;

      var w = new SwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "ON", Link = new Uri(@"http://link") },
        Mappings = new List<Mapping>(new[] { new Mapping() { Command = "1", Label = "Turn on" } })
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      await w.PostCommand.ExecuteAsync(null);
      Assert.That(command, Is.EqualTo("1"));
    }

    [Test]
    public async Task PostCommand_SetsProgressIndicator_Visible()
    {
       string command = string.Empty;
      _vmHelper.RestClientMock.Setup(
        x => x.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
        .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted)));
      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { State = "OFF", Link = new Uri(@"http:\\link") } }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      
      await w.PostCommand.ExecuteAsync(null);

      Assert.That(w.IsProgressIndicatorVisible, Is.True);
    }

    [Test]
    public async Task PostCommand_NotSetsTaskIndicatorVisible_WhenWidgetSingleMappingValue()
    {
      _vmHelper.RestClientMock.Setup(
        x => x.PostAsync(It.IsAny<Uri>(), It.IsAny<StringContent>()))
        .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted)));

      var w = new SwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "ON", Link = new Uri(@"http:\\link") },
        Mappings = new List<Mapping>(new[] { new Mapping() { Command = "1", Label = "Turn on" } })
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      
      await w.PostCommand.ExecuteAsync(null);
      
      Assert.That(w.IsProgressIndicatorVisible, Is.False);
    }

    [Test]
    public async Task PostCommand_ShowsErrorMessage_OnPostException()
    {
      string command = string.Empty;
      _vmHelper.RestClientMock.Setup(
        x => x.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
        .Throws<Exception>();

      var w = new SwitchWidgetModel(new Widget() { Item = new Item() { State = "OFF", Link = new Uri(@"http:\\link") } }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      await w.PostCommand.ExecuteAsync(null);
      
      _vmHelper.NavigationMock.Verify(x=>x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()));
    }

    [Test]
    public void Constructor_DoesNotSubscribeToItemChanged_WhenSwitchIsButton()
    {
      var w = new SwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "OFF", Link = new Uri("http://some")},
        Mappings = new List<Mapping>(new[] { new Mapping() { Command = "1", Label = "Turn on" } })
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);

      Assert.That(
        typeof (WidgetModelBase)
        .GetField("_itemChanged", BindingFlags.NonPublic | BindingFlags.Instance)
        .GetValue(w),
        Is.Null);
    }

    [Test]
    public void Constructor_SubscribesToItemChanged_WhenSwitchIsNormalSwitch()
    {
      var w = new SwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "OFF", Link = new Uri("http://some") }
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);

      Assert.That(
        typeof(WidgetModelBase)
        .GetField("_itemChanged", BindingFlags.NonPublic | BindingFlags.Instance)
        .GetValue(w),
        Is.Not.Null);
    }

    class TestSwitchWidgetModel : SwitchWidgetModel
    {
      public TestSwitchWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation) : base(data, clientFactory, navigation)
      {
      }

      public void CallOnItemChanged(Item e)
      {
        OnItemChanged(e);
      }

      public bool IsOnItemChangedCalled = false;
      protected override void OnItemChanged(Item e)
      {
        base.OnItemChanged(e);
        IsOnItemChangedCalled = true;
      }
    }

    [Test]
    public void EventChangedEventSubscriber_AndCallsSetState()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetLongPollingAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
        .Returns(JsonResources.SwitchItem.AsAsyncResponse);

      var w = new TestSwitchWidgetModel(new Widget()
      {
        Item = new Item() { State = "OFF", Link = new Uri("http://some") }
      }, _vmHelper.ClientFactory, _vmHelper.Navigation);
      w.ShowProgressIndicator();
      w.CallOnItemChanged(new Item() { State = "ON" });

      Assert.That(w.Data.Item.State, Is.EqualTo("ON"));
      Assert.That(w.IsProgressIndicatorVisible, Is.False);
    }
  }
}