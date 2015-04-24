using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinHAB.Core.Fx;
using WinHAB.Core.Localization;

namespace WinHAB.Core.Configuration
{
  public class AppConfiguration
  {
    protected IConfigurationProvider Provider { get; set; }

    public AppConfiguration(IConfigurationProvider provider)
    {
      Provider = provider;
      Runtime = new AppRuntimeConfiguration();
      UserResources = new UserResources();
    }

    public AppRuntimeConfiguration Runtime { get; set; }
    
    public UserResources UserResources { get; set; }
    public Uri UserResourcesUri { get { return new Uri((Server??"")+"/winhab/WinHAB.Resources.json");}}

    public void Load() { Provider.Load(); }
    public async Task LoadAsync() { await Provider.LoadAsync(); }

    public void Save() { Provider.Save(); }
    public async Task SaveAsync() { await Provider.SaveAsync(); }

    public string Language
    {
      get { return Provider.Get(this.GetPropertyName(() => Language))??"Auto"; }
      set { Provider.Set(this.GetPropertyName(() => Language), value); }
    }

    public IEnumerable<Language> AvailableLanguages
    {
      get
      {
        return new[] {new Language(Strings.LabelLanguageAuto, "Auto"),  new Language("English", "en")};
      }
    }

    public string Server
    {
      get { return Provider.Get(this.GetPropertyName(() => Server)); }
      set { Provider.Set(this.GetPropertyName(() => Server), value); }
    }

    public string Sitemap
    {
      get { return Provider.Get(this.GetPropertyName(() => Sitemap)); }
      set { Provider.Set(this.GetPropertyName(() => Sitemap), value); }
    }

    
  }
}