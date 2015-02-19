using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.Net;

namespace WinHAB.Tests.Core.Net
{
  [TestFixture]
  public class RestClientExtensionsTest
  {
    #region Test Fixture Setup

    [TestFixtureSetUp]
    public void SetupTestFixture()
    {
    }

    [TestFixtureTearDown]
    public void CleanupTestFixture()
    {
    }

    #endregion

    #region Test Setup
    [TearDown]
    public void CleanupTest()
    {
    }
    #endregion

    [SetUp]
    public void SetupTest()
    {
      _helper = new TestHelpers();
      _sitemapResponse = _helper.CreateAsyncResponse(JsonResources.Sitemaps);
    }

    private TestHelpers _helper;
    private Task<HttpResponseMessage> _sitemapResponse;

    [Test]
    public async Task AsJObjectAsync_ReturnsValidJObject_OnValidResponse()
    {
      var jobject = await _sitemapResponse.AsJObjectAsync();
      Assert.That(jobject, Is.InstanceOf<JObject>());
    }

    [Test]
    public void AsJObject_ThrowsException_WhenReponseIsNotValid()
    {
      var response = _helper.CreateAsyncResponse("<This is XML, NOT json>");
      Assert.That(async () => await response.AsJObjectAsync(), Throws.Exception);
    }

    [Test]
    public async Task AsSitemapListAsync_ReturnsSitemap_OnValidResponse()
    {
      var sitemap = await _sitemapResponse.AsSitemapListAsync();

      Assert.That(sitemap, Is.InstanceOf<List<Sitemap>>() & Is.Not.Null);
    }

    [Test]
    public async Task AsSitemapListAsync_ReturnsListOfSiteMapWithSigleItem_WhenResponseIsValid()
    {
      var sitemap = await _sitemapResponse.AsSitemapListAsync();
      
      Assert.That(sitemap.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task AsSitemapListAsync_SuccessfullyParsesSitemapsJson()
    {
      var sitemaps = await _sitemapResponse.AsSitemapListAsync();

      var sitemap = sitemaps[0];

      Assert.That(sitemap.Name, Is.EqualTo("demo"));
      Assert.That(sitemap.Label, Is.EqualTo("Main Menu"));
      Assert.That(sitemap.Link, Is.EqualTo(new Uri("http://localhost:8080/rest/sitemaps/demo"))); 
      Assert.That(sitemap.HomepageLink, Is.EqualTo(new Uri("http://localhost:8080/rest/sitemaps/demo/demo")));
    }
  }
}