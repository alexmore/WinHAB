using System;

namespace WinHAB.Core
{
  public interface IRestClientFactory
  {
    void SetBaseAddress(string newBaseAddress);

    IRestClient Create();
  }
}