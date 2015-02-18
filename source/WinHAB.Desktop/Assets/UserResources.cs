using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WinHAB.Core.Net;
using WinHAB.Desktop.Fx.Net;

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
          var resource = await cln.GetAsync(new Uri(serverAddress + "/winhab/UserResources.xaml")).AsResourceDictionaryAsync();
          if (resource != null)
            Icons.MergedDictionaries.Add(resource);
        }
      }
      catch
      {
      }
    }
  }
}