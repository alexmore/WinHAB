using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.ViewModels.Settings
{
  public class CustomIconsService : ObservableObject
  {
    private readonly string _iconsFile;

    public class CustomIcon : ObservableObject
    {
      public CustomIcon()
      {
        
      }
      public CustomIcon(string key, string pathData)
      {
        Key = key;
        PathData = pathData;
      }
      private string _Key;
      public string Key { get { return _Key; } set { _Key = value; RaisePropertyChanged(()=>Key); }}

      private string _PathData;
      public string PathData { get { return _PathData; } set { _PathData = value; RaisePropertyChanged(()=>PathData); }}
    }

    public CustomIconsService(string iconsFile)
    {
      _iconsFile = iconsFile;
      Icons = new ObservableCollection<CustomIcon>();
    }

    private ObservableCollection<CustomIcon> _Icons;
    public ObservableCollection<CustomIcon> Icons { get { return _Icons; } set { _Icons = value; RaisePropertyChanged(()=>Icons); }}

    public void Load()
    {
      if (!File.Exists(_iconsFile)) return;

      CustomIcon[] icons = null;

      icons = JsonConvert.DeserializeObject<CustomIcon[]>(File.ReadAllText(_iconsFile));

      if (icons != null && icons.Length != 0)
        Icons = new ObservableCollection<CustomIcon>(icons);
    }

    public void Save()
    {
      File.WriteAllText(_iconsFile, JsonConvert.SerializeObject(Icons.ToArray()));
    }

    public void Add(string key, string data)
    {
      Icons.Add(new CustomIcon(key, data));
    }

    public void Delete(string key)
    {
      if (Icons == null) return;
      var icon = Icons.FirstOrDefault(x => x.Key == key);
      if (icon != null)
        Icons.Remove(icon);
    }
  }
}