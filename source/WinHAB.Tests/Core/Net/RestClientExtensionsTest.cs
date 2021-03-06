﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WinHAB.Core.Models;
using WinHAB.Core.Net;
using WinHAB.Tests.Core.ViewModels;

namespace WinHAB.Tests.Core.Net
{
  [TestFixture]
  public class RestClientExtensionsTest
  {
    [SetUp]
    public void Setup()
    {
      _vmHelper = new ViewModelsTestHelper();
    }

    private ViewModelsTestHelper _vmHelper;


    [Test]
    public async Task AsJObjectAsync_ReturnsValidJObject_OnValidResponse()
    {
      var jobject = await JsonResources.Sitemaps.AsAsyncResponse().AsJObjectAsync();
      Assert.That(jobject, Is.InstanceOf<JObject>());
    }

    [Test]
    public void AsJObject_ThrowsException_WhenReponseIsNotValid()
    {
      var response = "<This is XML, NOT json>".AsAsyncResponse();
      Assert.That(async () => await response.AsJObjectAsync(), Throws.Exception);
    }

    [Test]
    public async Task AsSitemapListAsync_ReturnsSitemapListWithSingleItem_OnValidResponse()
    {
      var sitemap = await JsonResources.Sitemaps.AsAsyncResponse().AsSitemapListAsync();

      Assert.That(sitemap, Is.InstanceOf<List<Sitemap>>() & Is.Not.Null); 
      Assert.That(sitemap.Count, Is.EqualTo(1));
    }
    
    [Test]
    public async Task AsSitemapListAsync_SuccessfullyParsesSitemapsJson()
    {
      var sitemaps = await JsonResources.Sitemaps.AsAsyncResponse().AsSitemapListAsync();

      var sitemap = sitemaps[0];

      Assert.That(sitemap.Name, Is.EqualTo("demo"));
      Assert.That(sitemap.Label, Is.EqualTo("Main Menu"));
      Assert.That(sitemap.Link, Is.EqualTo(new Uri("http://localhost:8080/rest/sitemaps/demo"))); 
      Assert.That(sitemap.HomepageLink, Is.EqualTo(new Uri("http://localhost:8080/rest/sitemaps/demo/demo")));
    }

    [Test]
    public async Task AsPageAsync_ReturnsPageInstance_OnValidResponse()
    {
      var page = await JsonResources.MainPage.AsAsyncResponse().AsPageAsync();

      Assert.That(page, Is.InstanceOf<Page>() & Is.Not.Null);
      
    }

    [Test]
    public async Task AsPageAsync_SuccessfullyParsesPageJson()
    {
      var page = await JsonResources.MainPage.AsAsyncResponse().AsPageAsync();

      Assert.That(page.Id, Is.EqualTo("demo"));
      Assert.That(page.Title, Is.EqualTo("Main Menu"));

      Assert.That(page.Widgets.Count, Is.EqualTo(4));
      Assert.That(page.Widgets, Has.All.InstanceOf<Widget>());
    }
  }
}