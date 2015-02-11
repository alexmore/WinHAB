using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace WinHAB.Core
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
  }
}