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
    private Mock<IRestClient> _restMock;
    private Mock<IRestClientFactory> _restFactoryMock;

    [TestInitialize]
    public void PrepareTest()
    {
      _restMock = new Mock<IRestClient>();
      _restFactoryMock = new Mock<IRestClientFactory>();
    }

    #region
    #endregion


    [TestMethod]
    public async Task OpenHabClientGetSitemapsTest()
    {
      string json = @"{
                        ""sitemap"":
                            [ {""name"":""demo"",""label"":""Demo House"",""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo"",
                                     ""homepage"":{""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo/demo"",""leaf"":""false""}},
                              {""name"":""demo1"",""label"":""Demo House 1"",""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo1"",
                                     ""homepage"":{""link"":""http://demo.openhab.org:8080/rest/sitemaps/demo1/demo1"",""leaf"":""false""}}]
                      }";
      
      _restMock.Setup(x => x.GetJObjectAsync(It.Is<string>(url => url.Contains("/sitemaps"))))
        .ReturnsAsync(JObject.Parse(json));
      
      _restFactoryMock.Setup(x => x.Create()).Returns(_restMock.Object);

      var cln = new OpenHabClient(_restFactoryMock.Object);
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

      _restMock.Setup(x => x.GetJObjectAsync(It.Is<string>(url => url.Contains("/sitemaps"))))
        .ReturnsAsync(JObject.Parse(json));

      _restFactoryMock.Setup(x => x.Create()).Returns(_restMock.Object);

      var cln = new OpenHabClient(_restFactoryMock.Object);
      var sitemaps = await cln.GetSitemapsAsync();

      Assert.AreEqual(1, sitemaps.Count);

      var s1 = sitemaps[0]; 
      Assert.IsNull(s1.HomepageLink);
    }
  }
}
