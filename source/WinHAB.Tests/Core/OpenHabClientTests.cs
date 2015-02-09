using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using WinHAB.Core;

namespace WinHAB.Tests.Core
{
  [TestClass]
  public class OpenHabClientTests
  {
    private TestEnvironment _env = new TestEnvironment();
    
    [TestInitialize]
    public void PrepareTest()
    {
      _env.Init();
    }

    #region
    #endregion


    [TestMethod]
    public async Task OpenHabClientGetSitemapsTest()
    {
      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<string>(url => url.Contains("/sitemaps"))))
        .ReturnsAsync(JObject.Parse(_env.JsonSitemaps));

      var cln = new OpenHabClient(_env.RestClientFactory);
      var sitemaps = await cln.GetSitemapsAsync();

      Assert.AreEqual(2, sitemaps.Count);

      var s1 = sitemaps[0]; var s2 = sitemaps[1];

      Assert.AreEqual("demo", s1.Name);
      Assert.AreEqual("Demo House", s1.Label);
      Assert.AreEqual(new Uri("http://demo.openhab.org:8080/rest/sitemaps/demo"), s1.Link);
      Assert.AreEqual(new Uri("http://demo.openhab.org:8080/rest/sitemaps/demo/demo"), s1.HomepageLink);

      Assert.AreEqual("demo1", s2.Name);
      Assert.AreEqual("Demo House 1", s2.Label);
      Assert.AreEqual(new Uri("http://demo.openhab.org:8080/rest/sitemaps/demo1"), s2.Link);
      Assert.AreEqual(new Uri("http://demo.openhab.org:8080/rest/sitemaps/demo1/demo1"), s2.HomepageLink);
    }

    [TestMethod]
    public async Task OpenHabClientGetSitemapsHomepageNullTest()
    {
      string json = @"{""sitemap"":{""name"":""demo"",""label"":""Demo House"",""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo""}}";

      _env.RestClientMock.Setup(x => x.GetJObjectAsync(It.Is<string>(url => url == "/rest/sitemaps")))
        .ReturnsAsync(JObject.Parse(json));

      var cln = new OpenHabClient(_env.RestClientFactory);
      var sitemaps = await cln.GetSitemapsAsync();

      Assert.AreEqual(1, sitemaps.Count);

      var s1 = sitemaps[0]; 
      Assert.IsNull(s1.HomepageLink);
    }

    [TestMethod]
    public void OpenHabClientSetServerUriTest()
    {
      bool isSetBaseUriCalled = false;
      _env.RestClientFactoryMock.Setup(x => x.SetBaseAddress(It.IsAny<string>())).Callback(() => isSetBaseUriCalled = true);

      _env.Client.SetServerAddress("http://server");
      Assert.IsTrue(isSetBaseUriCalled);
    }
  }
}
