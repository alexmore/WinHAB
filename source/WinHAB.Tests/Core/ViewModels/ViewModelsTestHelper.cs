using System;
using Moq;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.Net;
using WinHAB.Core.ViewModels.Pages;

namespace WinHAB.Tests.Core.ViewModels
{
  public class ViewModelsTestHelper
  {
    public ViewModelsTestHelper()
    {
      ConfigurationProvider = new FakeConfigurationProvider();
      Configuration = new AppConfiguration(ConfigurationProvider);
      
      ClientFactoryMock = new Mock<IRestClientFactory>();
      RestClientMock = new Mock<IRestClient>();
      ClientFactoryMock.Setup(x => x.Create()).Returns(() => RestClientMock.Object);

      NavigationMock = new Mock<INavigationService>();
    }

    public FakeConfigurationProvider ConfigurationProvider { get; set; }
    public AppConfiguration Configuration { get; set; }
    
    public Mock<IRestClientFactory> ClientFactoryMock { get; set; }
    public Mock<IRestClient> RestClientMock { get; set; }

    public Mock<INavigationService> NavigationMock { get; set; }
  }
}