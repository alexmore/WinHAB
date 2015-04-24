using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.IO;
using WinHAB.Desktop.Fx.Windows;
using WinHAB.Desktop.Localization;

namespace WinHAB.Desktop.ViewModels.Settings
{
  public class SettingsIconsViewModel : ViewModel, ITitledViewModel
  {
    private readonly INavigationService _navigation;
    private readonly DesktopConfiguration _config;

    public SettingsIconsViewModel(INavigationService navigation, DesktopConfiguration config)
    {
      _navigation = navigation;
      _config = config;
      
      Title = Strings.TitleIcons;
      
      AddCustomIconCommand = new RelayCommand(AddCustomIcon);
      EditCustomIconCommand = new RelayCommand<UserResourcesData.UserResourcesIcon>(EditCustomIcon);
      DeleteCustomIconCommand = new RelayCommand<string>(DeleteCustomIcon);

      ExportCommand = new AsyncRelayCommand(Export);
    }

    public override async Task InitializeAsync(object parameter)
    {
      var defaultIconsDictionary = new ResourceDictionary() { Source = new Uri("/Assets/WidgetIcons.xaml", UriKind.Relative) };
      var defaultIcons =
        defaultIconsDictionary.GetResourceKeys()
          .Where(x => defaultIconsDictionary[x] as Geometry != null)
          .OrderBy(x => x)
          .Select(x => new KeyValuePair<string, Geometry>(x, (defaultIconsDictionary[x] as Geometry)));
      DefaultIcons = new ObservableCollection<KeyValuePair<string, Geometry>>(defaultIcons);

      try
      {
        var f = new FileInfo(_config.Constants.UserResourcesFile);
        if (f.Exists)
        {
          var fileContent = await f.ReadAllTextAsync(Encoding.UTF8);
          UserResources = new UserResources();
          UserResources.LoadFromString(fileContent);
        }
      }
      catch
      {
      }

      if (UserResources == null) UserResources = new UserResources();
    }

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(()=>Title); }}

    private UserResources _UserResources;
    public UserResources UserResources { get { return _UserResources; } set { _UserResources = value; RaisePropertyChanged(()=>UserResources); }}
    
    private ObservableCollection<KeyValuePair<string, Geometry>> _DefaultIcons;
    public ObservableCollection<KeyValuePair<string, Geometry>> DefaultIcons { get { return _DefaultIcons; } set { _DefaultIcons = value; RaisePropertyChanged(() => DefaultIcons); } }
    
    public RelayCommand AddCustomIconCommand { get; set; }
    public RelayCommand<UserResourcesData.UserResourcesIcon> EditCustomIconCommand { get; set; }
    public RelayCommand<string> DeleteCustomIconCommand { get; set; }

    private async Task SaveUserResources(string fileName = null)
    {
      if (fileName.IsNullOrWhitespace()) fileName = _config.Constants.UserResourcesFile;
      await new FileInfo(fileName).WriteAllTextAsync(UserResources.Data.ToString(), FileMode.Create, Encoding.UTF8);
    }

    private async void AddCustomIcon()
    {
      string[] keys = null;
      if (UserResources.Data.Icons != null)
        keys = UserResources.Data.Icons.Select(x => x.Key).ToArray();
      var vm = await _navigation.NavigateAsync<SettingsIconsAddIconDialogModel>(new SettingsIconsAddIconDialogModel.SettingsIconsAddIconDialogModelParameter(keys, null, null));

      if (await vm.DialogResult())
      {
        if (UserResources.Data.Icons != null)
          UserResources.Data.Icons.Add(new UserResourcesData.UserResourcesIcon(vm.Key, vm.PathData));
        await SaveUserResources();
      }
    }

    private async void EditCustomIcon(UserResourcesData.UserResourcesIcon icon)
    {
      if (icon == null) return;

      string[] keys = null;
      if (UserResources.Data.Icons != null)
        keys = UserResources.Data.Icons.Select(x => x.Key).ToArray();

      var vm = await _navigation.NavigateAsync<SettingsIconsAddIconDialogModel>(new SettingsIconsAddIconDialogModel.SettingsIconsAddIconDialogModelParameter(keys, icon.Key, icon.PathData));
      if (await vm.DialogResult())
      {
        icon.Key = vm.Key;
        icon.PathData = vm.PathData;
        await SaveUserResources();
      }
      
    }

    private async void DeleteCustomIcon(string key)
    {
      if (await _navigation.ShowQuestionAsync(Strings.TitleEditIconDialog, Strings.MessageQuestionDeleteCustomIcon))
      {
        if (UserResources.Data.Icons == null) return;
        var icon = UserResources.Data.Icons.FirstOrDefault(x => x.Key == key);
        if (icon != null)
          UserResources.Data.Icons.Remove(icon);
        await SaveUserResources();
      }
    }

    public AsyncRelayCommand ExportCommand { get; set; }

    // TODO: Replace SaveFileDialog with INavigationService
    private async Task Export()
    {
      var sd = new SaveFileDialog();
      sd.AddExtension = true;
      sd.CheckPathExists = true;
      sd.FileName = "WinHAB.Resources.json";
      sd.DefaultExt = "*.json";
      sd.Filter = "*.json|*.json";
      sd.OverwritePrompt = true;
      var res = sd.ShowDialog();
      if (res.HasValue && res.Value)
      {
        await SaveUserResources(sd.FileName);
      }
      
    }

  }
}