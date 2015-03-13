using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Tests.Core.ViewModels.Pages
{
  [TestFixture]
  public class BootstrapperPageModelTest
  {
    [SetUp]
    public void SetupTest()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    BootstrapperPageModel CreateBootstrapperPageModel()
    {
      return new BootstrapperPageModel(_vmHelper.NavigationMock.Object, _vmHelper.ClientFactoryMock.Object,
        _vmHelper.Configuration);
    }
    
    private ViewModelsTestHelper _vmHelper;

    [Test]
    public async Task InitializeAsync_ShowsServerAddressInput_WhenServerAddressIsEmpty()
    {
      var pm = CreateBootstrapperPageModel();

      await pm.InitializeAsync(null);

      AssertIsAddressInputVisible(pm);
    }

    void AssertIsAddressInputVisible(BootstrapperPageModel pm)
    {
      Assert.That(pm.IsServerAddressVisible, Is.True);
      Assert.That(pm.IsSitemapsVisible, Is.False);
      Assert.That(pm.IsProgressIndicatorVisible, Is.False);
    }

    [Test]
    public async Task InitializeAsync_SetsDefaultServerAddress_WhenServerAddressIsEmpty()
    {
      var pm = CreateBootstrapperPageModel();
      await pm.InitializeAsync(null);
      Assert.That(pm.ServerAddress, Is.EqualTo("http://"));
    }

    [Test]
    public async Task InitializeAsync_ShowsServerAddressInput_WhenApplicationRestaringAndServerAddressIsNotEmpty()
    {
      var pm = CreateBootstrapperPageModel();
      _vmHelper.Configuration.Server = "http://localhost";
      _vmHelper.Configuration.Runtime.IsRestarting = true;
      await pm.InitializeAsync(null);

      AssertIsAddressInputVisible(pm);
    }

    [Test]
    public async Task InitializeAsync_InvokesConnectCommand_WhenAddressServerIsNotEmptyAndNotRestarting()
    {
      _vmHelper.Configuration.Server = "http://localhost";

      var pm = CreateBootstrapperPageModel();
      bool isCommandCalled = false;
      pm.ConnectCommand = new AsyncRelayCommand<string>(x => { isCommandCalled = true; return Task.FromResult(0); });
      await pm.InitializeAsync(null);

      Assert.That(isCommandCalled, Is.True);
    }

    [Test]
    public void ConnectCommand_CanNotExecute_WhenServerAddressIsEmpty()
    {
      var vm = CreateBootstrapperPageModel();
      Assert.That(vm.ConnectCommand.CanExecute("stub"), Is.False);
    }

    [Test]
    public void ConnectCommand_CanExecute_WhenServerAddressIsNotEmpty()
    {
      var vm = CreateBootstrapperPageModel();
      vm.ServerAddress = "some server";
      Assert.That(vm.ConnectCommand.CanExecute("stub"), Is.True);
    }

    private void PrepareVMHelperForConnectCommandExecution()
    {
      _vmHelper.RestClientMock.Setup(x => x.GetAsync(It.Is<Uri>(u => u == new Uri("http://localhost/rest/sitemaps/") || u == new Uri("http://localhost1/rest/sitemaps/"))))
        .Returns(JsonResources.Sitemaps.AsAsyncResponse());

      _vmHelper.Configuration.Server = "http://localhost";
    }

    void AssertIsSitemapSelectorVisible(BootstrapperPageModel pm)
    {
      Assert.That(pm.IsServerAddressVisible, Is.False);
      Assert.That(pm.IsSitemapsVisible, Is.True);
      Assert.That(pm.IsProgressIndicatorVisible, Is.False);
    }

    [Test]
    public async Task ConnectCommand_SavesServerAddressToConfiguration()
    {
      PrepareVMHelperForConnectCommandExecution();

      var vm = CreateBootstrapperPageModel();
      await vm.ConnectCommand.ExecuteAsync("http://localhost1");

      Assert.That(_vmHelper.Configuration.Server, Is.EqualTo("http://localhost1"));
    }

    [Test]
    public async Task ConnectCommandExecuteAsync_ShowsSitemapsSelector_WhenSitemapIsEmptyInConfiguration()
    {
      PrepareVMHelperForConnectCommandExecution();
      
      var vm = CreateBootstrapperPageModel();
      await vm.ConnectCommand.ExecuteAsync("http://localhost");

      AssertIsSitemapSelectorVisible(vm);
    }

     [Test]
    public async Task ConnectCommand_ShowsSitemapsSelector_WhenSitemapListNotContainsConfigurationSitemap()
    {
      PrepareVMHelperForConnectCommandExecution();

      var vm = CreateBootstrapperPageModel();
       _vmHelper.Configuration.Sitemap = "demo1";
      await vm.ConnectCommand.ExecuteAsync("http://localhost");

      AssertIsSitemapSelectorVisible(vm);
    }

    [Test]
    public async Task ConnectCommand_LoadsSitemaps_WhenValidServerAddress()
    {
      PrepareVMHelperForConnectCommandExecution();

      var vm = CreateBootstrapperPageModel();
      await vm.ConnectCommand.ExecuteAsync("http://localhost");

      Assert.That(vm.Sitemaps, Is.Not.Null);
      Assert.That(vm.Sitemaps.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ConnectCommand_ShowsErrorMessage_WhenAnyError()
    {
      PrepareVMHelperForConnectCommandExecution();
      
      var vm = CreateBootstrapperPageModel();
      await vm.ConnectCommand.ExecuteAsync("http://unavailable_host");

      _vmHelper.NavigationMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()));
    }

    [Test]
    public async Task ConnectCommand_CallsSelectSitemapCommand_WhenConfigurationHasSitemap()
    {
      PrepareVMHelperForConnectCommandExecution();

      var vm = CreateBootstrapperPageModel();
      bool isCommandCalled = false;
      vm.SelectSitemapCommand = new AsyncRelayCommand<Sitemap>(x => { isCommandCalled = true; return Task.FromResult(0); });
      _vmHelper.Configuration.Sitemap = "demo";
      await vm.ConnectCommand.ExecuteAsync("http://localhost");

      Assert.That(isCommandCalled, Is.True);
    }

    [Test]
    public async Task SelectSitemapComand_ShowsErrorMessage_WhenSitemapIsNull()
    {
      var vm = CreateBootstrapperPageModel();
      await vm.SelectSitemapCommand.ExecuteAsync(null);

      _vmHelper.NavigationMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()));
    }

    [Test]
    public async Task SelectSitemapComand_ShowsErrorMessage_WhenSitemapHomepageLinkIsNull()
    {
      var vm = CreateBootstrapperPageModel();
      await vm.SelectSitemapCommand.ExecuteAsync(new Sitemap());

      _vmHelper.NavigationMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()));
    }


    [Test]
    public async Task SelectSitemapComand_SavesSelectedSitemapNameToConfiguration_WhenSitemapLoadingSuccessful()
    {
      var vm = CreateBootstrapperPageModel();
      _vmHelper.Configuration.Sitemap = null;
      
      await vm.SelectSitemapCommand.ExecuteAsync(new Sitemap() { Name= "demo", HomepageLink = new Uri("http://localhost")});
      
      Assert.That(_vmHelper.Configuration.Sitemap, Is.EqualTo("demo"));
    }

    [Test]
    public async Task SelectSitemapCommand_NavigatesToMainPageModel_WhenSitemapLoadingSuccessful()
    {
      var vm = CreateBootstrapperPageModel();

      await vm.SelectSitemapCommand.ExecuteAsync(new Sitemap() { Name = "demo", HomepageLink = new Uri("http://localhost") });

      _vmHelper.NavigationMock.Verify(x => x.NavigateAsync<MainPageModel>(It.IsAny<Sitemap>()));
    }

    [Test]
    public async Task SelectSitemapCommand_ShowsErrorMessageAndSitemapSelector_WhenNavigationToMainPageModelFails()
    {
      var vm = CreateBootstrapperPageModel();
      _vmHelper.NavigationMock.Setup(x => x.NavigateAsync<MainPageModel>(It.IsAny<Sitemap>())).Throws<Exception>();
      
      await vm.SelectSitemapCommand.ExecuteAsync(new Sitemap() { Name = "demo", HomepageLink = new Uri("http://localhost") });

      AssertIsSitemapSelectorVisible(vm);
      _vmHelper.NavigationMock.Verify(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()));
    }
   
  }
}