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
  public class SelectionWidgetModelTest
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
    public void Constructor_ThrowsArgumentException_WhenDataIsNull()
    {
      Assert.That(
        () =>
          new SelectionWidgetModel(null,
            _vmHelper.ClientFactory, _vmHelper.Navigation),
        Throws.ArgumentException);

    }
    
    [Test]
    public void Constructor_ThrowsArgumentException_WhenMappingsIsNull()
    {
      Assert.That(
        () =>
          new SelectionWidgetModel(new Widget(), 
            _vmHelper.ClientFactory, _vmHelper.Navigation),
        Throws.ArgumentException);

    }

    [Test]
    public void Constructor_ThrowsArgumentException_WhenMappingsHasLessThanTwoItems()
    {
      Assert.That(
        () =>
          new SelectionWidgetModel(new Widget() { Mappings = new List<Mapping>(new [] { new Mapping() })},
            _vmHelper.ClientFactory, _vmHelper.Navigation),
        Throws.ArgumentException);

    }

    [Test]
    public void Constructor_SetsSize_ToMedium()
    {
      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Meduim));
    }

    [Test]
    public void Constructor_SetsSizeToWide_WhenPropertiesSizeIsWide()
    {
      _widget.Label = "{ Size = Wide }";
      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Wide));
    }

    [Test]
    public void Constructor_SetsSizeToLarge_WhenPropertiesSizeIsLarge()
    {
      _widget.Label = "{ Size = Large }";
      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Large));
    }

    [Test]
    public void Constructor_SetsSizeToMedium_WhenPropertiesSizeIsWrong()
    {
      _widget.Label = "{ Size = VeryWide }";
      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Meduim));
    }

    [Test]
    public void Constructor_CreatesMappings_ObservableCollection()
    {
      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Mappings, Is.EqualTo(_widget.Mappings));
    }

    [Test]
    public void Constructor_UpdatesValue()
    {
      _widget.Item.State = "2";

      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation); 
      
      Assert.That(w.Value, Is.EqualTo("Map 2"));
      Assert.That(w.SelectedMapping.Label, Is.EqualTo("Map 2"));
      
    }

    [Test]
    public void Constructor_UpdatesValueToNull_WhenStateIsUnknown()
    {
      _widget.Item.State = "0";

      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      Assert.That(w.Value, Is.Null);
      Assert.That(w.SelectedMapping, Is.Null);

    }

    [Test]
    public void UpdateValue_SuspendPostSelection()
    {
      _widget.Item.State = "1";

      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      _vmHelper.RestClientMock.Verify(x=>x.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()), Times.Never());
    }

    [Test]
    public void UpdateValue_SetsIsOffState_WhenMappingPropertiesHasIsOff()
    {
      _widget.Item.State = "1";
      _widget.Mappings[0] = new Mapping() { Command = "1", Label = "Map 1 {IsOff}" };

      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      Assert.That(w.IsOffState, Is.True);
    }

    [Test]
    public void UpdateValue_SetsIsOffStateFalse_WhenMappingPropertiesNotHasIsOff()
    {
      _widget.Item.State = "2";
      _widget.Mappings[0] = new Mapping() { Command = "1", Label = "Map 1 {IsOff}" };

      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      Assert.That(w.IsOffState, Is.False);
    }

    [Test]
    public void SelectedMapping_Invokes_PostingSelection()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("2", c))))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.Item.State = "1";
      var w = new SelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      w.SelectedMapping = new Mapping() {Label = "Map 2", Command = "2"};

      _vmHelper.RestClientMock.Verify(
        x => x.PostAsync(
          It.IsAny<Uri>(),
          It.Is<StringContent>(c => _vmHelper.CheckStringContent("2", c))
          ));
    }

    [Test]
    public void Constructor_SubscribesToStateChanged()
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

    class TestSelectionWidgetModel : SelectionWidgetModel
    {
      public TestSelectionWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
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
      var w = new TestSelectionWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      w.ShowProgressIndicator();
      w.CallOnItemChanged(new Item() { State = "2" });

      Assert.That(w.Data.Item.State, Is.EqualTo("2"));
      Assert.That(w.IsProgressIndicatorVisible, Is.False);
      Assert.That(w.Value, Is.EqualTo("Map 2"));
      Assert.That(w.SelectedMapping.Label, Is.EqualTo("Map 2"));
    }

  }
}