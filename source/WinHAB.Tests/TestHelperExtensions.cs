using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WinHAB.Tests
{
  public static class TestHelperExtensions
  {
    public static Task<HttpResponseMessage> AsAsyncResponse(this string content)
    {
      return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {Content = new StringContent(content)});
    }
  }
}