using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinHAB.Core.Mvvm;

namespace WinHAB.Tests.Core.Mvvm
{
  [TestClass]
  public class WaiterViewModelTests
  {
    [TestMethod]
    public void WaiterViewModelShowHideTests()
    {
      var vm = new WaiterViewModel();
      vm.Show();
      Assert.IsTrue(vm.IsVisible);
      vm.Hide();
      Assert.IsFalse(vm.IsVisible);
      
      var message = "Hello world!";
      vm.Show(message);
      Assert.IsTrue(vm.IsVisible);
      Assert.AreEqual(message, vm.Text);
      vm.Hide();
      Assert.IsNull(vm.Text);
    }
  }
}
