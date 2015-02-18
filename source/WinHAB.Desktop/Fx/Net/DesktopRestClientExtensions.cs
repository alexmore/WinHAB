using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WinHAB.Core.Net;

namespace WinHAB.Desktop.Fx.Net
{
  public static class DesktopRestClientExtensions
  {
    public static async Task<ResourceDictionary> AsResourceDictionaryAsync(this Task<HttpResponseMessage> responce)
    {
      var stream = await responce.AsStreamAsync();
      try
      {
        var reader = new System.Windows.Markup.XamlReader();
        return (ResourceDictionary)reader.LoadAsync(stream);
      }
      catch
      {
        return null;
      }
    }
  }
}