using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;

namespace WinHAB.Core.Configuration
{
  public class UserResourcesData : ObservableObject
  {
    public class UserResourcesIcon : ObservableObject
    {
      public UserResourcesIcon()
      {
        
      }
      
      public UserResourcesIcon(string key, string pathData)
      {
        Key = key;
        PathData = pathData;
      }

      private string _Key;
      public string Key { get { return _Key; } set { _Key = value; RaisePropertyChanged(()=>Key); }}
      private string _PathData;
      public string PathData { get { return _PathData; } set { _PathData = value; RaisePropertyChanged(()=>PathData); }}
    }

    public UserResourcesData()
    {
      Icons = new ObservableCollection<UserResourcesIcon>();
    }

    private ObservableCollection<UserResourcesIcon> _Icons;
    public ObservableCollection<UserResourcesIcon> Icons { get { return _Icons; } set { _Icons = value; RaisePropertyChanged(()=>Icons); }}

    public override string ToString()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}