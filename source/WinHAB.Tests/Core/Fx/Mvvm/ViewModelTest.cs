using NUnit.Framework;
using WinHAB.Core.Fx.Mvvm;

namespace WinHAB.Tests.Core.Fx.Mvvm
{
  [TestFixture]
  public class ViewModelTest
  {
    [Test]
    public void Constructor_Creates_TaskProgressInstance()
    {
      var vm = new ViewModel();
      Assert.That(vm.TaskProgress, Is.Not.Null);
    }
  }
}