using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Net
{
  public interface IRestClient : IDisposable
  {
    Task<HttpResponseMessage> GetAsync(Uri query);
  }
}