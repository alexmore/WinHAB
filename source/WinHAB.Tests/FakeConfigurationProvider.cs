using System.Collections.Generic;
using System.Threading.Tasks;
using WinHAB.Core.Configuration;

namespace WinHAB.Tests
{
  public class FakeConfigurationProvider : IConfigurationProvider
  {
    public FakeConfigurationProvider()
    {
      Values = new Dictionary<string, string>();
    }

    public Dictionary<string, string> Values { get; set; }

    public virtual void Load() { }
    public virtual Task LoadAsync() { return Task.FromResult(0); }
    public virtual void Save() { }
    public virtual Task SaveAsync() { return Task.FromResult(0); }

    public string Get(string key)
    {
      if (!Values.ContainsKey(key)) return null;
      
      return Values[key];
    }

    public void Set(string key, string value)
    {
      Values[key] = value;
    }
  }
}