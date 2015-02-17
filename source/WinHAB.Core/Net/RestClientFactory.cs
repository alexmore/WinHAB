namespace WinHAB.Core.Net
{
  public class RestClientFactory : IRestClientFactory
  {
    public IRestClient Create()
    {
      return new RestClient();
    }
  }
}