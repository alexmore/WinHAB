using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core.Net
{
  public interface IRestClient : IDisposable
  {
    Task<JObject> GetJObjectAsync(Uri query);
    Task<Stream> GetAsStreamAsync(Uri query);
  }
}