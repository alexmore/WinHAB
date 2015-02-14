using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WinHAB.Desktop.Configuration
{
  public static class UserResources
  {
    public static ResourceDictionary Icons { get; private set; }
    
    public static async Task LoadUserResources(string serverAddress)
    {
      await Task.Run(() =>
      {
        try
        {
          Icons = new ResourceDictionary();
          Icons.Source = new Uri(serverAddress + "/winhab/UserResources.xaml");
        }
        catch (Exception)
        {
        }
      });
    }
  }
}