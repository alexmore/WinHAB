using System;

namespace WinHAB.Core
{
  public interface IRestClientFactory
  {
    IRestClient Create();
  }
}