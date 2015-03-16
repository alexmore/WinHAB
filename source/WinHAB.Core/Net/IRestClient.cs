using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Net
{
  public interface IRestClient : IDisposable
  {
    Task<HttpResponseMessage> GetAsync(Uri query);
    Task<HttpResponseMessage> GetLongPollingAsync(Uri query, CancellationToken cancellationToken);


    Task<HttpResponseMessage> PostAsync(Uri query, HttpContent content);
  }
}