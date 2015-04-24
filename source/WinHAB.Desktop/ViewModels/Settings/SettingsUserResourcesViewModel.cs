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
  public class SettingsUserResourcesViewModel : ViewModel, ITitledViewModel
  {
    private readonly INavigationService _navigation;
    private readonly DesktopConfiguration _config;

    public SettingsUserResourcesViewModel(INavigationService navigation, DesktopConfiguration config)
    {
      _navigation = navigation;
      _config = config;
      
      Title = Strings.TitleUserResources;
      ResourceFile = config.Constants.UserResourcesFile;
      
      AddCustomIconCommand = new RelayCommand(AddCustomIcon);
      EditCustomIconCommand = new RelayCommand<UserResourcesData.UserResourcesIcon>(EditCustomIcon);
      DeleteCustomIconCommand = new RelayCommand<string>(DeleteCustomIcon);
      
      OpenCommand = new AsyncRelayCommand(Open);
      SaveAsCommand = new AsyncRelayCommand(SaveAs);
      OpenDefaultCommand = new AsyncRelayCommand(OpenDefault);
    }

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(() => Title); } }

    private string _ResourceFile;
    public string ResourceFile { get { return _ResourceFile; } set { _ResourceFile = value; RaisePropertyChanged(()=>ResourceFile); }}

    private UserResources _UserResources;
    public UserResources UserResources { get { return _UserResources; } set { _UserResources = value; RaisePropertyChanged(() => UserResources); } }

    public RelayCommand AddCustomIconCommand { get; set; }
    public RelayCommand<UserResourcesData.UserResourcesIcon> EditCustomIconCommand { get; set; }
    public RelayCommand<string> DeleteCustomIconCommand { get; set; }
    public AsyncRelayCommand SaveAsCommand { get; set; }
    public AsyncRelayCommand OpenCommand { get; set; }
    public AsyncRelayCommand OpenDefaultCommand { get; set; }

    public override async Task InitializeAsync(object parameter)
    {
      await LoadUserResources(ResourceFile);
    }

    // TODO: Make load/save safely
    private async Task LoadUserResources(string fileName)
    {
      UserResources = new UserResources();
      try
      {
        var f = new FileInfo(fileName);
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
    }

    private async Task SaveUserResources(string fileName = null)
    {
      if (fileName.IsNullOrWhitespace()) fileName = ResourceFile;
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
      if (await _navigation.ShowQuestionAsync(Strings.TitleEditIconDialog, string.Format(Strings.MessageQuestionDeleteIconFormat, key)))
      {
        if (UserResources.Data.Icons == null) return;
        var icon = UserResources.Data.Icons.FirstOrDefault(x => x.Key == key);
        if (icon != null)
          UserResources.Data.Icons.Remove(icon);
        await SaveUserResources();
      }
    }

    private async Task OpenDefault()
    {
      ResourceFile = _config.Constants.UserResourcesFile;
      await LoadUserResources(ResourceFile);
    }

    // TODO: Replace SaveFileDialog with INavigationService
    private async Task Open()
    {
      var od = new OpenFileDialog();
      od.CheckFileExists = true;
      od.CheckPathExists = true;
      od.DefaultExt = "*.json";
      od.Multiselect = false;
      od.Filter = "WinHAB Configuration Files (*.json)|*.json";

      var res = od.ShowDialog();
      if (res.HasValue && res.Value)
      {
        await LoadUserResources(od.FileName);
        ResourceFile = od.FileName;
      }
    }

    // TODO: Replace SaveFileDialog with INavigationService
    private async Task SaveAs()
    {
      var sd = new SaveFileDialog();
      sd.AddExtension = true;
      sd.CheckPathExists = true;
      sd.FileName = "WinHAB.Resources.json";
      sd.DefaultExt = "*.json";
      sd.Filter = "WinHAB Configuration Files (*.json)|*.json";
      sd.OverwritePrompt = true;
      var res = sd.ShowDialog();
      if (res.HasValue && res.Value)
      {
        await SaveUserResources(sd.FileName);
        ResourceFile = sd.FileName;
      }
      
    }

  }
}