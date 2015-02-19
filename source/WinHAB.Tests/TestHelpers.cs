using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WinHAB.Tests
{
  public class TestHelpers
  {
    public Task<HttpResponseMessage> CreateAsyncResponse(string content)
    {
      return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(content)});
    }
  }
}