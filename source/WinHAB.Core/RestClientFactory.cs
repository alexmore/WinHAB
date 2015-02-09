using System;

namespace WinHAB.Core
{
  public class RestClientFactory : IRestClientFactory
  {
    private string _baseAddress;

    public RestClientFactory(string baseAddress)
    {
      _baseAddress = baseAddress;
    }

    public void SetBaseAddress(string newBaseAddress)
    {
      _baseAddress = newBaseAddress;
    }

    public IRestClient Create()
    {
      return new RestClient(new Uri(_baseAddress));
    }
  }
}