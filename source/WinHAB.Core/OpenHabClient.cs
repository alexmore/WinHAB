using System.Collections.Generic;
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

    public async Task<List<SitemapData>> GetSitemapsAsync()
    {
      using (var cln = _factory.Create())
      {
        var jobject = await cln.GetJObjectAsync("/sitemaps");
        if (jobject == null) return null;

        return jobject.ToObject<SitemapListData>().Sitemaps;
      }
    }
  }
}