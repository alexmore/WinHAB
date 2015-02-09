using System;

namespace WinHAB.Core
{
  public class RestClientFactory : IRestClientFactory
  {
    private Uri _baseUri;

    public RestClientFactory(Uri baseUri)
    {
      _baseUri = baseUri;
    }

    public void SetBaseUri(Uri newBaseUri)
    {
      _baseUri = newBaseUri;
    }

    public IRestClient Create()
    {
      return new RestClient(_baseUri);
    }
  }
}