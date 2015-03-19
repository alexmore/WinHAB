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
  public class SetpointWidgetModelTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
      _widget = new Widget()
      {
        MinValue = 10, MaxValue = 20, Step = 1,
        Item = new Item() {  Link = new Uri("http://some")}
      };
    }

    private ViewModelsTestHelper _vmHelper;
    private Widget _widget;

    [Test]
    public void Constructor_ThrowsArgumentException_WhenMinValueGraterThanMaxValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 20;
      Assert.That(
        () => new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation),
        Throws.ArgumentException);
    }

    [Test]
    public void Constructor_SetsStepToOn1_WhenStepIsZero()
    {
      _widget.Step = 0;
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      Assert.That(w.Data.Step, Is.EqualTo(1));
    }

    [Test]
    public void Constructor_SetsSize_ToMedium_WhenSizeIsNull()
    {
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Meduim));
    }

    [Test]
    public void Constructor_SetsSize_ToWide()
    {
      _widget.Label = "{Size=Wide}";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Wide));
    }

    [Test]
    public void Constructor_SetsSize_ToWide_WhenSizeIsLarge()
    {
      _widget.Label = "{Size=Large}";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.Size, Is.EqualTo(WidgetSize.Wide));
    }

    [Test]
    public void Constructor_SetsValue_FromState()
    {
      _widget.Item.State = "25.5";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.DecimalValue, Is.EqualTo(25.5));
      Assert.That(w.Value, Is.EqualTo(w.DecimalValue.ToString()));
    }

    [Test]
    public void Constructor_CreatesCommands()
    {
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.UpCommand, Is.Not.Null); 
      Assert.That(w.DownCommand, Is.Not.Null);
    }

    [Test]
    public void IsValueMax_IsTrue_WhenDecimalValueGreaterEqualsToMaxValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "10";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMax, Is.True);

      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "11";
      w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMax, Is.True);
      
    }

    [Test]
    public void IsValueMax_IsFalse_WhenDecimalValueNotEqualsToMaxValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "9";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMax, Is.False);
    }

    [Test]
    public void IsValueMax_IsFalse_WhenIsUnlimitedRange()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 10;
      _widget.Item.State = "10";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMax, Is.False);
    }

    [Test]
    public void IsValueMin_IsTrue_WhenDecimalValueLessOrEqualsToMinValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "5";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMin, Is.True);

      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "4";
      w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMin, Is.True);
    }

    [Test]
    public void IsValueMin_IsFalse_WhenDecimalValueNotEqualsToMinValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "6";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMin, Is.False);
    }

    [Test]
    public void IsValueMin_IsFalse_WhenIsUnlimitedRange()
    {
      _widget.MaxValue = 5;
      _widget.MinValue = 5;
      _widget.Item.State = "5";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      Assert.That(w.IsValueMin, Is.False);

    }

    [Test]
    public async Task UpCommand_DoesntPost_WhenValueEqualsToMaxValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "10";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.UpCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(x=>x.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()), Times.Never);
    }

    [Test]
    public async Task UpCommand_Post_WhenValueEqualsToMaxValueAndUnlimitedRange()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 10;
      _widget.Item.State = "10";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.UpCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(x => x.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()));
    }

    [Test]
    public async Task UpCommand_Post_DecimalValuePlusStep()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "9";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.UpCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("10", c))));
    }

    [Test]
    public async Task DownCommand_DoesntPost_WhenValueEqualsToMinValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "5";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.DownCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(x => x.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()), Times.Never);
    }

    [Test]
    public async Task DownCommand_Post_WhenValueEqualsToMinValueAndUnlimitedRange()
    {
      _widget.MaxValue = 5;
      _widget.MinValue = 5;
      _widget.Item.State = "5";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.DownCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(x => x.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()));
    }

    [Test]
    public async Task DownCommand_Post_DecimalValueMinusStep()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "9";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.DownCommand.ExecuteAsync(null);

      _vmHelper.RestClientMock.Verify(x => x.PostAsync(It.IsAny<Uri>(),
        It.Is<StringContent>(c => _vmHelper.CheckStringContent("8", c))));
    }

    [Test]
    public async Task Post_ShowsProgressIndicator()
    {
      _vmHelper.RestClientMock.Setup(x => x.PostAsync(It.IsAny<Uri>(),
        It.IsAny<HttpContent>()))
        .Returns(_vmHelper.OkHttpResonseMessageTask);

      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "9";
      var w = new SetpointWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);

      await w.DownCommand.ExecuteAsync(null);

      Assert.That(w.IsProgressIndicatorVisible, Is.True);
    }

    class TestSetpointnWidgetModel : SetpointWidgetModel
    {
      public TestSetpointnWidgetModel(Widget data, IRestClientFactory clientFactory, INavigationService navigation)
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
    public void ItemChangedRaising_UpdatesValue()
    {
      _widget.MaxValue = 10;
      _widget.MinValue = 5;
      _widget.Item.State = "9";
      var w = new TestSetpointnWidgetModel(_widget, _vmHelper.ClientFactory, _vmHelper.Navigation);
      w.ShowProgressIndicator();
      w.CallOnItemChanged(new Item() { State = "8" });

      Assert.That(w.Data.Item.State, Is.EqualTo("8"));
      Assert.That(w.IsProgressIndicatorVisible, Is.False);
      Assert.That(w.Value, Is.EqualTo(w.DecimalValue.ToString()));
    }
  }
}