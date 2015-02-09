using System.Collections.Generic;
using System.Threading.Tasks;
using WinHAB.Core.Configuration;

namespace WinHAB.Tests
{
  public class TestConfigurationProvider : IConfigurationProvider
  {
    public TestConfigurationProvider()
    {
      Values = new Dictionary<string, string>();
    }

    public Dictionary<string, string> Values { get; set; }
    public bool IsLoaded { get; set; }
    public bool IsSaved { get; set; }

    public void Load()
    {
      IsLoaded = true;
    }

    public Task LoadAsync()
    {
      IsLoaded = true;
      return Task.FromResult(0);
    }

    public void Save()
    {
      IsSaved = true;
    }

    public Task SaveAsync()
    {
      IsSaved = true;
      return Task.FromResult(0);
    }

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