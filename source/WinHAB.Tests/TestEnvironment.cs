using System;
using System.Threading.Tasks;
using Moq;
using WinHAB.Core;
using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;
using WinHAB.Desktop;

namespace WinHAB.Tests
{
  public class TestEnvironment
  {
    public Mock<IRestClient> RestClientMock { get; set; }
    public IRestClient RestClient { get { return RestClientMock.Object; }}
    public Mock<IRestClientFactory> RestClientFactoryMock { get; set; }
    public IRestClientFactory RestClientFactory { get { return RestClientFactoryMock.Object; }}
    public OpenHabClient Client { get; set; }

    public Mock<INavigationHost> NavigationHostMock { get; set; }
    public INavigationHost NavigationHost { get { return NavigationHostMock.Object; }}

    public Mock<INavigationService> NavigationMock { get; set; }
    public INavigationService Navigation { get { return NavigationMock.Object; }}

    public Mock<AbstractViewModelViewFactory> ViewModelViewFactoryMock { get; set; }
    public AbstractViewModelViewFactory ViewModelViewFactory { get { return ViewModelViewFactoryMock.Object; }}

    public TestConfigurationProvider ConfigurationProvider { get; set; }
    public AppConfiguration AppConfiguration { get; set; }

    public void Init()
    {
      RestClientMock = new Mock<IRestClient>();
      RestClientFactoryMock = new Mock<IRestClientFactory>();
      RestClientFactoryMock.Setup(x => x.Create()).Returns(RestClient);

      Client = new OpenHabClient(RestClientFactory);

      NavigationHostMock = new Mock<INavigationHost>();
      NavigationHostMock.SetupAllProperties();
      ViewModelViewFactoryMock = new Mock<AbstractViewModelViewFactory>();
      ViewModelViewFactoryMock.CallBase = true;
      NavigationMock = new Mock<INavigationService>();
      InitNavigation();

      ConfigurationProvider = new TestConfigurationProvider();
      AppConfiguration = new AppConfiguration(ConfigurationProvider);
    }

    #region Navigation Environment

    public Action<IViewModel> OnNavigationNavigate { get; set; }
    public Action<string, string> OnNavigationShowMessage { get; set; }

    void InitNavigation()
    {
      NavigationMock.Setup(x => x.Navigate(It.IsAny<IViewModel>()))
        .Callback<IViewModel>(vm => { if (OnNavigationNavigate != null) OnNavigationNavigate(vm); });

      NavigationMock.Setup(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
        .Returns(Task.FromResult(true))
        .Callback<string, string>((t, tt) => { if (OnNavigationShowMessage != null) OnNavigationShowMessage(t, tt); });

      NavigationMock.Setup(x => x.ShowMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action>()))
       .Callback<string, string, Action>((t, tt, a) =>
       {
         if (a != null) a();
         if (OnNavigationShowMessage != null) OnNavigationShowMessage(t, tt);
       });
    }
    
    #endregion

    #region Json strings
    public readonly string JsonSitemaps = @"{ ""sitemap"":
                            [ {""name"":""demo"",""label"":""Demo House"",""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo"",
                                     ""homepage"":{""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo/demo"",""leaf"":""false""}},
                              {""name"":""demo1"",""label"":""Demo House 1"",""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo1"",
                                     ""homepage"":{""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo1/demo1"",""leaf"":""false""}}]}";
    #endregion
  }
}