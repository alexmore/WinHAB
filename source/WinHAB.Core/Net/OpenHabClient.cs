using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WinHAB.Core.Models;

namespace WinHAB.Core.Net
{
  public class OpenHabClient
  {
    private readonly IRestClientFactory _factory;

    public OpenHabClient(IRestClientFactory restClientFactory)
    {
      _factory = restClientFactory;
    }

    public async Task<List<Sitemap>> GetSitemapsAsync(Uri sitemapUri)
    {
      using (var cln = _factory.Create())
      {
        var jobject = await cln.GetJObjectAsync(sitemapUri);
        if (jobject == null) return null;

        return jobject.ToObject<SitemapList>().Sitemaps;
      }
    }

    public async Task<Page> GetPageAsync(Uri pageUri)
    {
      using (var cln = _factory.Create())
      {
        var jobject = await cln.GetJObjectAsync(pageUri);
        if (jobject == null) return null;
        
        return jobject.ToObject<Page>();
      }
    }

    public async Task<Stream> GetStreamAsync(Uri uri)
    {
      using (var cln = _factory.Create())
      {
        return await cln.GetAsStreamAsync(uri);
      }
    }
  }
}