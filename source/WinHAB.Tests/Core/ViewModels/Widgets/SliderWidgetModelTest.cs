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
  public class SliderWidgetModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
      _widget = new Widget()
      {
        Mappings = new List<Mapping>(new []{ new Mapping() { Label = "Map 1", Command = "1"}, new Mapping() { Label = "Map 2", Command = "2"} }),
        Item = new Item() { Link = new Uri("http://some")}
      };
    }

    private ViewModelsTestHelper _vmHelper;
    private Widget _widget;

    [Test]
    public void Constructor_SetsSize_ToMedium()
    {
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Meduim));
    }

    [Test]
    public void Constructor_SetsSizeToWide_WhenPropertiesSizeIsWide()
    {
      _widget.Label = "{ Size = Wide }";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Wide));
    }

    [Test]
    public void Constructor_SetsSizeToLarge_WhenPropertiesSizeIsLarge()
    {
      _widget.Label = "{ Size = Large }";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Wide));
    }

    [Test]
    public void Constructor_SetsSizeToMedium_WhenPropertiesSizeIsWrong()
    {
      _widget.Label = "{ Size = VeryWide }";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Meduim));
    }

    [Test]
    public void Constructor_SetsPercentValue()
    {
      _widget.Item.State = "25";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.PercentValue, Is.EqualTo(25));
    }

    [Test]
    public void Constructor_SetsPercentValueToNull_WhenStateIsNotNumber()
    {
      _widget.Item.State = "Hello";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.PercentValue, Is.Null);
    }

    [Test]
    public void Constructor_SetsPercentValueToMax_WhenStateIsLarge()
    {
      _widget.Item.State = "150";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.PercentValue, Is.EqualTo(100));
    }

    [Test]
    public void Constructor_SetsPercentValueToMin_WhenStateIsLess()
    {
      _widget.Item.State = "-20";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.PercentValue, Is.EqualTo(0));
    }

    [Test]
    public void PercentValueChanging_InvokesPercentPost()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("20", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = "1";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      w.PercentValue = 20;

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("20", c))
          ));
    }

    [Test]
    public async Task OffCommand_PostsOffAndUpdatesPercentValue()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("OFF", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = "1";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.OffCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("OFF", c))
          ));
    }

    [Test]
    public async Task OnCommand_PostsOffAndUpdatesPercentValue()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = "1";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.OnCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))
          ));
    }

    [Test]
    public async Task OnOffCommand_PostsOffWhenPercentValueIsGreaterThan0()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("OFF", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = "1";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.OnOffCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("OFF", c))
          ));
    }

    [Test]
    public async Task OnOffCommand_PostsOnWhenPercentValueIs0()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = "0";
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.OnOffCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))
          ));
    }

    [Test]
    public async Task OnOffCommand_PostsOnWhenPercentValueIsNull()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = null;
      var w = new SliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.OnOffCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("ON", c))
          ));
    }

    class TestSliderWidgetModel : SliderWidgetModel
    {
      public TestSliderWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
        : base(data, clientFactory, navigation)
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
      _widget.Item.State = "1";
      var w = new TestSliderWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      w.ShowProgressIndicator();
      w.CallOnItemChanged(new Item() { State = "2" });

      Assert.That(w.Data.Item.State, Is.EqualTo("2"));
      Assert.That(w.IsProgressIndicatorVisible, Is.False);
      Assert.That(w.Value, Is.EqualTo("2%"));
      Assert.That(w.PercentValue, Is.EqualTo(2));
    }



  }
}