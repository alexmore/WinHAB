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
    public static async Task<JObject> AsJObjectAsync(this Task<HttpResponseMessage> responceTask)
    {
      var responce = await responceTask;
      responce.EnsureSuccessStatusCode();
      return JObject.Parse(await responce.Content.ReadAsStringAsync());
    }

    public static async Task<Stream> AsStreamAsync(this Task<HttpResponseMessage> responceTask)
    {
      var responce = await responceTask;
      responce.EnsureSuccessStatusCode();
      return await responce.Content.ReadAsStreamAsync();
    }

    public static async Task<List<Sitemap>> AsSitemapAsync(this Task<HttpResponseMessage> responceTask)
    {
      var jobject = await responceTask.AsJObjectAsync();
      if (jobject == null) return null;

      return jobject.ToObject<SitemapList>().Sitemaps;
    }

    public static async Task<Page> AsPageAsync(this Task<HttpResponseMessage> responceTask)
    {
      var jobject = await responceTask.AsJObjectAsync();
      if (jobject == null) return null;

      return jobject.ToObject<Page>();
    }


  }
}