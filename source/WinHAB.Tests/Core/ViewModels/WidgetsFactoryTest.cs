﻿using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.ViewModels;
using WinHAB.Core.ViewModels.Widgets;

namespace WinHAB.Tests.Core.ViewModels
{
  [TestFixture]
  public class WidgetFactoryTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;

    [Test]
    public void Create_ReturnsSwitchWidgetModel_WhenWidgetTypeIsSwtichAndMappingsIsEmpty()
    {
      var wd = new Widget() {Type = WidgetType.Switch, Item = new Item() { Link = new Uri("http://some")}};

      Func<Type, Widget, WidgetModelBase> cw = (tp, wp) => new SwitchWidgetModel(wd, _vmHelper.ClientFactory, _vmHelper.Navigation);
      var wf = new WidgetsFactory(cw);

      var w = wf.Create(wd);

      Assert.That(w, Is.InstanceOf<SwitchWidgetModel>());
    }

    [Test]
    public void Create_ReturnsSwitchWidgetModel_WhenWidgetTypeIsSwtichAndMappingsIsSingle()
    {
      var wd = new Widget()
      {
        Type = WidgetType.Switch,
        Mappings = new List<Mapping>(new [] { new Mapping() { Command = "1", Label = "1"}}),
        Item = new Item() { Link = new Uri("http://some") }
      };

      Func<Type, Widget, WidgetModelBase> cw = (tp, wp) => new SwitchWidgetModel(wd, _vmHelper.ClientFactory, _vmHelper.Navigation);
      var wf = new WidgetsFactory(cw);

      var w = wf.Create(wd);

      Assert.That(w, Is.InstanceOf<SwitchWidgetModel>());
    }

    [Test]
    public void Create_ReturnsSelectionWidgetModel_WhenWidgetTypeIsSwtichAndMappingsCountGreaterThanOne()
    {
      var wd = new Widget()
      {
        Type = WidgetType.Switch,
        Item = new Item() { Link = new Uri("http://some")},
        Mappings = new List<Mapping>(new[] { new Mapping() { Command = "1", Label = "1" }, new Mapping() { Command = "1", Label = "1" } })
      };

      Func<Type, Widget, WidgetModelBase> cw = (tp, wp) =>
      {
        if (tp == typeof(SelectionWidgetModel)) return new SelectionWidgetModel(wd, _vmHelper.ClientFactory, _vmHelper.Navigation);
        
        return new SwitchWidgetModel(wd, _vmHelper.ClientFactory, _vmHelper.Navigation);
      };
      
      var wf = new WidgetsFactory(cw);

      var w = wf.Create(wd);

      Assert.That(w, Is.InstanceOf<SelectionWidgetModel>());
    }

    [Test]
    public void Create_ReturnsNull_WhenItemTypeIsRollershutter()
    {
      var wd = new Widget()
      {
        Type = WidgetType.Switch,
        Item = new Item() {Type = ItemType.Rollershutter}
      };

      Func<Type, Widget, WidgetModelBase> cw = (tp, wp) => new SwitchWidgetModel(wd, _vmHelper.ClientFactory, _vmHelper.Navigation);
      var wf = new WidgetsFactory(cw);

      var w = wf.Create(wd);

      Assert.That(w, Is.Null);
    }
  }
}