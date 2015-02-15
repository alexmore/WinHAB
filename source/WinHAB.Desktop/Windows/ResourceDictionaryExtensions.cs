using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WinHAB.Desktop.Windows
{
  public static class ResourceDictionaryExtensions
  {
    public static IEnumerable<string> GetResourceKeys(this ResourceDictionary resource)
    {
      var keys = resource.Keys.Cast<string>().ToList();

      foreach (var i in resource.MergedDictionaries)
        keys.AddRange(i.GetResourceKeys());

      return keys;
    }
  }
}