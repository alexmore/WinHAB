using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Net
{
  public class RestClient : HttpClient, IRestClient
  {
    public RestClient()
    {
    }

    protected void CreateHeaders()
    {
      DefaultRequestHeaders.Clear();
      DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public new Task<HttpResponseMessage> GetAsync(Uri query)
    {
      CreateHeaders();
      return base.GetAsync(query);
    }

    public new Task<HttpResponseMessage> PostAsync(Uri query, HttpContent content)
    {
      CreateHeaders();
      return base.PostAsync(query, content);
    }

    public Task<HttpResponseMessage> GetLongPollingAsync(Uri query, CancellationToken cancellationToken)
    {
      CreateHeaders();
      DefaultRequestHeaders.Add("X-Atmosphere-Transport", "long-polling");
      Timeout = System.Threading.Timeout.InfiniteTimeSpan;

      return base.GetAsync(query, cancellationToken);
    }
  }
}