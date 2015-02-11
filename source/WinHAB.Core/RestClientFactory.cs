using System;

namespace WinHAB.Core
{
  public class RestClientFactory : IRestClientFactory
  {
    public IRestClient Create()
    {
      return new RestClient();
    }
  }
}