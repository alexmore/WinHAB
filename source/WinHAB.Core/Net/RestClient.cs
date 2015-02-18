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
  }
}