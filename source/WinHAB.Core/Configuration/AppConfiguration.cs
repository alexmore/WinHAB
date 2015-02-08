using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinHAB.Core.Configuration
{
  public class AppConfiguration
  {
    private readonly IConfigurationProvider _provider;

    public AppConfiguration(IConfigurationProvider provider)
    {
      _provider = provider;
    }

    public void Load() { _provider.Load(); }
    public async Task LoadAsync() { await _provider.LoadAsync(); }

    public void Save() { _provider.Save(); }
    public async Task SaveAsync() { await _provider.SaveAsync(); }

    public string Language
    {
      get { return _provider.Get(this.GetPropertyName(() => Language)) ?? "en"; }
      set { _provider.Set(this.GetPropertyName(() => Language), value); }
    }

    public IEnumerable<Language> AvailableLanguages
    {
      get
      {
        return new[] {new Language("English", "en"), new Language("Русский", "ru"), new Language("Deutsch", "de")};
      }
    }

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