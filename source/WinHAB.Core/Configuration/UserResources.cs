using System;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinHAB.Core.Net;

namespace WinHAB.Core.Configuration
{
  public class UserResources : ObservableObject
  {
    public UserResources()
    {
      Data = new UserResourcesData();
    }

    private UserResourcesData _Data;
    public UserResourcesData Data { get { return _Data; } private set { _Data = value; RaisePropertyChanged(()=>Data); }}

    public async Task LoadAsync(Uri resourcesUri, IRestClientFactory clientFactory)
    {
      Data = new UserResourcesData();
      try
      {
        using (var cln = clientFactory.Create())
        {
          var response = await cln.GetAsync(resourcesUri);
          if (response.IsSuccessStatusCode)
          {
            Data = JObject.Parse(await response.Content.ReadAsStringAsync()).ToObject<UserResourcesData>();
          }
        }
      }
      catch
      {
      }
    }

    public void LoadFromString(string resourceContent)
    {
      Data = new UserResourcesData();
      try
      {
        Data = JObject.Parse(resourceContent).ToObject<UserResourcesData>();
      }
      catch
      {
      }
    }
  }
}