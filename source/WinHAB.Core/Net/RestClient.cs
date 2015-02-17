using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Net
{
  public class RestClient : HttpClient, IRestClient
  {
    public RestClient()
    {
      DefaultRequestHeaders.Clear();
      DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<JObject> GetJObjectAsync(Uri query)
    {
      HttpResponseMessage responce = await GetAsync(query);
      responce.EnsureSuccessStatusCode();
      
      return JObject.Parse(await responce.Content.ReadAsStringAsync());
    }

    public async Task<Stream> GetAsStreamAsync(Uri query)
    {
      HttpResponseMessage responce = await GetAsync(query);
      responce.EnsureSuccessStatusCode();

      return await responce.Content.ReadAsStreamAsync();

    }
  }
}