using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WinHAB.Core.Net;

namespace WinHAB.Desktop.Assets
{
  public static class UserResources
  {
    public static ResourceDictionary Icons { get; private set; }

    public static async Task LoadUserResources(string serverAddress, IRestClientFactory clientFactory)
    {
      Icons = new ResourceDictionary();

      Icons.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Themes/WidgetIcons.xaml", UriKind.Relative) });

      try
      {
        using (var cln = clientFactory.Create())
        {
          var stream = await cln.GetAsync(new Uri(serverAddress + "/winhab/UserResources.xaml")).AsStreamAsync();
          var userResourcesReader = new System.Windows.Markup.XamlReader();
          Icons.MergedDictionaries.Add(LoadReasourceDictionary(stream));
        }
      }
      catch
      {
      }
    }

    public static ResourceDictionary LoadReasourceDictionary(Stream stream)
    {
      try
      {
        var reader = new System.Windows.Markup.XamlReader();
        return (ResourceDictionary) reader.LoadAsync(stream);
      }
      catch
      {
        
      }

      return null;
    }
  }
}