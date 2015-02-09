using System;

namespace WinHAB.Core
{
  public class RestClientFactory : IRestClientFactory
  {
    private readonly Uri _baseUri;

    public RestClientFactory(Uri baseUri)
    {
      _baseUri = baseUri;
    }

    public IRestClient Create()
    {
      return new RestClient(_baseUri);
    }

    public IRestClient Create(string baseUrl)
    {
      return new RestClient(new Uri(baseUrl));
    }
  }
}