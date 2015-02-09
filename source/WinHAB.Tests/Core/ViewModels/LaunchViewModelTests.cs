using System;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Core.ViewModels;

namespace WinHAB.Tests.Core.ViewModels
{
  [TestClass]
  public class LaunchViewModelTests
  {
    private readonly TestEnvironment _env = new TestEnvironment();

    [TestInitialize]
    public void InitTest()
    {
      _env.Init();
    }

    [TestMethod]
    public async Task ConnectCommandTest()
    {
      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<string>(url => url.Contains("/rest/sitemaps"))))
        .ReturnsAsync(JObject.Parse(_env.JsonSitemaps));

      string factoryAddress = null;
      _env.RestClientFactoryMock.Setup(x => x.SetBaseAddress(It.IsAny<string>()))
        .Callback<string>(x => factoryAddress = x);
      
      var vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);

      const string server = "http://server/";
      _env.ConfigurationProvider.IsSaved = false;

      await vm.ConnectCommand.ExecuteAsync(server);
      Assert.AreEqual(2, vm.Sitemaps.Count);
      Assert.IsTrue(vm.IsSitemapsVisible);
      Assert.IsFalse(vm.IsServerUrlVisible);
      Assert.IsFalse(vm.Waiter.IsVisible);
      Assert.AreEqual(server, factoryAddress);
      Assert.AreEqual(server, _env.AppConfiguration.Server);
      Assert.IsTrue(_env.ConfigurationProvider.IsSaved);
    }

    [TestMethod]
    public async Task ConnectCommandExceptionTest()
    {
      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<string>(url => url.Contains("/rest/sitemaps"))))
        .Throws(new Exception("Some exception"));
      
      var vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);

      bool isExceptionCatched = false;
      _env.OnNavigationShowMessage = (t, tt) => isExceptionCatched = true;

      await vm.ConnectCommand.ExecuteAsync("http://server/");

      Assert.IsTrue(isExceptionCatched);
      Assert.IsFalse(vm.IsSitemapsVisible);
      Assert.IsTrue(vm.IsServerUrlVisible);
      Assert.IsFalse(vm.Waiter.IsVisible);
      
    }
  }
}