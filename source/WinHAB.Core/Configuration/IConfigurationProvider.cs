using System.Threading.Tasks;

namespace WinHAB.Core.Configuration
{
  public interface IConfigurationProvider
  {
    void Load();
    Task LoadAsync();

    void Save();
    Task SaveAsync();

    string Get(string key);
    void Set(string key, string value);

    
  }
}