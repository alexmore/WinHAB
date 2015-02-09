using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinHAB.Core.Mvvm;

namespace WinHAB.Tests.Core.Mvvm
{
  
  [TestClass]
  public class AbstractViewModelViewFactoryTests
  {

    class View1 : IView
    {
      public object DataContext { get; set; }
    }

    class View2 : IView
    {
      public object DataContext { get; set; }
    }

    class ViewModel1 : IViewModel
    {
      public int Parameter1;

      public ViewModel1(int parameter1)
      {
        Parameter1 = parameter1;
      }

      public void Cleanup()
      {
        throw new NotImplementedException();
      }

      public INavigationService Navigation { get; private set; }
      public void OnNavigatedTo()
      {
        throw new NotImplementedException();
      }
    }

    class ViewModel2 : IViewModel
    {
      public void Cleanup()
      {
        throw new NotImplementedException();
      }

      public INavigationService Navigation { get; private set; }
      public void OnNavigatedTo()
      {
        throw new NotImplementedException();
      }
    }

    class ImplementationOfAbstractViewModelViewFactoryForTests : AbstractViewModelViewFactory
    {
      public List<KeyValuePair<Type, Type>> GetMapList()
      {
        return MapList;
      }

      public override IViewModel CreateViewModel(Type viewModelType, Action<ConstructorParameters> ctorParameters)
      {
        if (viewModelType == typeof(ViewModel1)) return new ViewModel1((int)ctorParameters.GetConstructorParameters().Parameters.Select(x=>x.Value).First());
        if (viewModelType == typeof(ViewModel2)) return new ViewModel2();

        return null;
      }

      public override IView CreateView(Type viewType)
      {
        if (viewType == typeof(View1)) return new View1();
        if (viewType == typeof(View2)) return new View2();

        return null;
      }
    }

    [TestMethod]
    public void AbstractViewModelViewFactoryCreateViewModelTest()
    {
      var factory = new ImplementationOfAbstractViewModelViewFactoryForTests();

      Assert.IsInstanceOfType(factory.CreateViewModel(typeof(ViewModel2)), typeof(ViewModel2));

      var vm = factory.CreateViewModel(typeof (ViewModel1), x => x.Add("p1", 10));
      Assert.IsInstanceOfType(vm, typeof(ViewModel1));
      Assert.AreEqual(10, (vm as ViewModel1).Parameter1);
    }

    [TestMethod]
    public void AbstractViewModelViewFactoryCreateViewTest()
    {
      var factory = new ImplementationOfAbstractViewModelViewFactoryForTests();
      Assert.IsInstanceOfType(factory.CreateView(typeof(View1)), typeof(View1));
    }

    [TestMethod]
    public void AbstractViewModelViewFactoryMapTest()
    {
      var factory = new ImplementationOfAbstractViewModelViewFactoryForTests();
      factory.Map<ViewModel1, View1>();
      factory.Map<ViewModel2, View2>();

      Assert.AreEqual(2, factory.GetMapList().Count);
    }

    [TestMethod]
    public void AbstractViewModelViewFactoryCreateViewForViewModelTest()
    {
      var factory = new ImplementationOfAbstractViewModelViewFactoryForTests();
      factory.Map<ViewModel1, View1>();
      factory.Map<ViewModel2, View2>();

      Assert.IsInstanceOfType(factory.CreateViewByViewModelType(typeof(ViewModel1)), typeof(View1));
    }


  }
}
