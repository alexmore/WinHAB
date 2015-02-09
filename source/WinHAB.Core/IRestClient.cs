using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinHAB.Core
{
  public interface IRestClient : IDisposable
  {
    Task<JObject> GetJObjectAsync(string query);
  }
}