using System;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Model;
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
    public async Task LaunchViewModelConnectOnNavigateTest()
    {
      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<Uri>(url => url.OriginalString.Contains("/demo/sitemaps"))))
        .ReturnsAsync(JObject.Parse(JsonResources.Sitemaps));
      
      // If AppConfiguration.Server is empty
      var vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);

      Assert.IsFalse(vm.IsServerAddressVisible);
      Assert.IsFalse(vm.IsSitemapsVisible);

      vm.OnNavigatedTo();
      Assert.AreEqual("http://", vm.ServerAddress);
      Assert.IsTrue(vm.IsServerAddressVisible);
      Assert.IsFalse(vm.IsSitemapsVisible);

      vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);
      _env.ConfigurationProvider.Values["Server"] = "http://myserver/";
      vm.OnNavigatedTo();
      Assert.AreEqual("http://myserver/", vm.ServerAddress);
    }

    [TestMethod]
    public async Task LaunchViewModelConnectCommandTest()
    {
      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<Uri>(url => url.OriginalString.Contains("/rest/sitemaps"))))
        .ReturnsAsync(JObject.Parse(JsonResources.Sitemaps));
      
      var vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);

      const string server = "http://server/";
      _env.ConfigurationProvider.IsSaved = false;

      vm.ServerAddress = server;
      await vm.ConnectCommand.ExecuteAsync(server);
      Assert.IsNotNull(vm.Sitemaps);
      Assert.AreEqual(2, vm.Sitemaps.Count);
      Assert.IsTrue(vm.IsSitemapsVisible);
      Assert.IsFalse(vm.IsServerAddressVisible);
      Assert.IsFalse(vm.Waiter.IsVisible);
      Assert.AreEqual(server, _env.AppConfiguration.Server);
      Assert.IsTrue(_env.ConfigurationProvider.IsSaved);
    }

    [TestMethod]
    public async Task LaunchViewModelConnectCommandExceptionTest()
    {
      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<Uri>(url => url.OriginalString.Contains("/demo/sitemaps"))))
        .Throws(new Exception("Some exception"));
      
      var vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);

      bool isExceptionCatched = false;
      _env.OnNavigationShowMessage = (t, tt) => isExceptionCatched = true;

      vm.ServerAddress = "http://server/";
      await vm.ConnectCommand.ExecuteAsync("http://server/");

      Assert.IsTrue(isExceptionCatched);
      Assert.IsFalse(vm.IsSitemapsVisible);
      Assert.IsTrue(vm.IsServerAddressVisible);
      Assert.IsFalse(vm.Waiter.IsVisible);
    }

    [TestMethod]
    public async Task LaunchViewModelSelectSitemapCommandTest()
    {
      _env.NavigationMock.Setup(x=>x.Navigate<MainViewModel>(It.IsAny<Action<ConstructorParameters>>()))
        .Verifiable();
      _env.NavigationMock.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()))
        .Verifiable();

      var vm = new LaunchViewModel(_env.Navigation, _env.Client, _env.AppConfiguration);
      vm.SelectSitemapCommand.Execute(new SitemapData());
      _env.NavigationMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()));

      vm.SelectSitemapCommand.Execute(new SitemapData() {HomepageLink = new Uri("http://hello")});
      _env.NavigationMock.Verify(x=>x.Navigate<MainViewModel>(It.IsAny<Action<ConstructorParameters>>()));
    }
  }
}