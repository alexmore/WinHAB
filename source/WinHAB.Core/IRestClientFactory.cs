using System;

namespace WinHAB.Core
{
  public interface IRestClientFactory
  {
    void SetBaseUri(Uri newBaseAddress);

    IRestClient Create();
  }
}