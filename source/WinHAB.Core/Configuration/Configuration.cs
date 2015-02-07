using System.Threading.Tasks;

namespace WinHAB.Core.Configuration
{
  public class Configuration
  {
    private readonly IConfigurationProvider _provider;

    public Configuration(IConfigurationProvider provider)
    {
      _provider = provider;
    }

    public void Load() { _provider.Load(); }
    public async Task LoadAsync() { await _provider.LoadAsync(); }

    public void Save() { _provider.Save(); }
    public async Task SaveAsync() { await _provider.SaveAsync(); }

    public string Server
    {
      get { return _provider.Get(this.GetPropertyName(() => Server)); }
      set { _provider.Set(this.GetPropertyName(() => Server), value); }
    }

    public string Sitemap
    {
      get { return _provider.Get(this.GetPropertyName(() => Sitemap)); }
      set { _provider.Set(this.GetPropertyName(() => Sitemap), value); }
    }
  }
}