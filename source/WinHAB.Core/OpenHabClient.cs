using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinHAB.Core.Model;

namespace WinHAB.Core
{
  public class OpenHabClient
  {
    private readonly IRestClientFactory _factory;

    public OpenHabClient(IRestClientFactory restClientFactory)
    {
      _factory = restClientFactory;
    }

    public async Task<List<SitemapData>> GetSitemapsAsync(Uri sitemapUri)
    {
      using (var cln = _factory.Create())
      {
        var jobject = await cln.GetJObjectAsync(sitemapUri);
        if (jobject == null) return null;

        return jobject.ToObject<SitemapListData>().Sitemaps;
      }
    }

    public async Task<PageData> GetPageAsync(Uri pageUri)
    {
      using (var cln = _factory.Create())
      {
        var jobject = await cln.GetJObjectAsync(pageUri);
        if (jobject == null) return null;
        
        return jobject.ToObject<PageData>();
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