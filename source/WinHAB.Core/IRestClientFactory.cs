namespace WinHAB.Core
{
  public interface IRestClientFactory
  {
    IRestClient Create();
    IRestClient Create(string baseUrl);
  }
}