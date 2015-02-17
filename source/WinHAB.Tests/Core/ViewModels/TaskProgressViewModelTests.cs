using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.ViewModels
{
  [TestClass]
  public class TaskProgressViewModelTests
  {
    [TestMethod]
    public void TaskProgressViewModelShowHideTests()
    {
      var vm = new TaskProgressViewModel();
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
