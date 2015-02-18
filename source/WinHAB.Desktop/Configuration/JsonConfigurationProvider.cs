using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinHAB.Core.Configuration;
using WinHAB.Desktop.Fx.IO;

namespace WinHAB.Desktop.Configuration
{
  public class JsonConfigurationProvider : IConfigurationProvider
  {
    private Dictionary<string, string> _values = new Dictionary<string, string>();
    private readonly string _fileName;

    public JsonConfigurationProvider(string fileName)
    {
      _fileName = fileName;
    }

    public void Load()
    {
      if (!File.Exists(_fileName))
        Save();
      _values = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_fileName));
    }

    public async Task LoadAsync()
    {
      var f = new FileInfo(_fileName);
      if (!f.Exists)
        await SaveAsync();

      var fileContent = await f.ReadAllTextAsync(Encoding.UTF8);
      _values = await Task.Run(() => JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent));
    }

    public void Save()
    {
      File.WriteAllText(_fileName, JsonConvert.SerializeObject(_values));
    }

    public async Task SaveAsync()
    {
      var serialized = await Task.Run(() => JsonConvert.SerializeObject(_values));
      await new FileInfo(_fileName).WriteAllTextAsync(serialized, FileMode.Create, Encoding.UTF8);
    }

    public string Get(string key)
    {
      if (!_values.ContainsKey(key)) return null;

      return _values[key];
    }

    public void Set(string key, string value)
    {
      _values[key] = value;
    }
  }
}