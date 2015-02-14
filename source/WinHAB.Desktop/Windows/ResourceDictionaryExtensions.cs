using System.Linq;
using System.Windows;

namespace WinHAB.Desktop.Windows
{
  public static class ResourceDictionaryExtensions
  {
    public static object FindResource(this ResourceDictionary resource, string key)
    {
      if (resource == null) return null;

      var resKey = resource.Keys.Cast<string>()
        .FirstOrDefault(x =>x.ToLower() == key.ToLower().Trim());
      
      return resKey != null ? resource[resKey] : null;
    }

    public static object FindIconResource(this ResourceDictionary resource, string key)
    {
      if (resource == null) return null;

      return resource.FindResource(key) ?? resource.FindResource(key + "icon");
    }
  }
}