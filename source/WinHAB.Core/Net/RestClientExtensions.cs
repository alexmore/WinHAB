using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WinHAB.Core.Models;

namespace WinHAB.Core.Net
{
  public static class RestClientExtensions
  {
    public static async Task<JObject> AsJObjectAsync(this Task<HttpResponseMessage> responseTask)
    {
      var response = await responseTask;
      response.EnsureSuccessStatusCode();
      return JObject.Parse(await response.Content.ReadAsStringAsync());
    }

    public static async Task<Stream> AsStreamAsync(this Task<HttpResponseMessage> responseTask)
    {
      var response = await responseTask;
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStreamAsync();
    }

    public static async Task<List<Sitemap>> AsSitemapListAsync(this Task<HttpResponseMessage> responseTask)
    {
      var jobject = await responseTask.AsJObjectAsync();
      if (jobject == null) return null;

      return jobject.ToObject<SitemapList>().Sitemaps;
    }

    public static async Task<Page> AsPageAsync(this Task<HttpResponseMessage> responseTask)
    {
      var jobject = await responseTask.AsJObjectAsync();
      if (jobject == null) return null;

      return jobject.ToObject<Page>();
    }


  }
}