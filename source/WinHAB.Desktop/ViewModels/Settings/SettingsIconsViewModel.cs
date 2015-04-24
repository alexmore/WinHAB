using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using WinHAB.Core.Configuration;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Desktop.Configuration;
using WinHAB.Desktop.Fx.Windows;
using WinHAB.Desktop.Localization;

namespace WinHAB.Desktop.ViewModels.Settings
{
  public class SettingsIconsViewModel : ViewModel, ITitledViewModel
  {
    private readonly INavigationService _navigation;

    public SettingsIconsViewModel(INavigationService navigation, DesktopConfiguration config)
    {
      _navigation = navigation;
      Title = Strings.TitleIcons;

      var defaultIconsDictionary = new ResourceDictionary() {Source = new Uri("/Assets/WidgetIcons.xaml", UriKind.Relative)};
      var defaultIcons =
        defaultIconsDictionary.GetResourceKeys()
          .Where(x => defaultIconsDictionary[x] as Geometry != null)
          .OrderBy(x=>x)
          .Select(x => new KeyValuePair<string, Geometry>(x, (defaultIconsDictionary[x] as Geometry)));
      DefaultIcons = new ObservableCollection<KeyValuePair<string, Geometry>>(defaultIcons);

      CustomIcons = new CustomIconsService(config.Constants.CustomIconsFile);
      CustomIcons.Load();
      
      AddCustomIconCommand = new RelayCommand(AddCustomIcon);
      EditCustomIconCommand = new RelayCommand<CustomIconsService.CustomIcon>(EditCustomIcon);
      DeleteCustomIconCommand = new RelayCommand<string>(DeleteCustomIcon);

      ExportXamlCommand = new RelayCommand(ExportXaml);
    }
    
    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(()=>Title); }}
    
    private ObservableCollection<KeyValuePair<string, Geometry>> _DefaultIcons;
    public ObservableCollection<KeyValuePair<string, Geometry>> DefaultIcons { get { return _DefaultIcons; } set { _DefaultIcons = value; RaisePropertyChanged(() => DefaultIcons); } }

    private CustomIconsService _CustomIcons;
    public CustomIconsService CustomIcons { get { return _CustomIcons; } set { _CustomIcons = value; RaisePropertyChanged(()=>CustomIcons); }}

    public RelayCommand AddCustomIconCommand { get; set; }
    public RelayCommand<CustomIconsService.CustomIcon> EditCustomIconCommand { get; set; }
    public RelayCommand<string> DeleteCustomIconCommand { get; set; }

    private async void AddCustomIcon()
    {
      string[] keys = null;
      if (CustomIcons.Icons != null)
        keys = CustomIcons.Icons.Select(x => x.Key).ToArray();
      var vm = await _navigation.NavigateAsync<SettingsIconsAddIconDialogModel>(new SettingsIconsAddIconDialogModel.SettingsIconsAddIconDialogModelParameter(keys, null, null));

      if (await vm.DialogResult())
      {
        CustomIcons.Add(vm.Key, vm.PathData);
        CustomIcons.Save();
      }
    }

    private async void EditCustomIcon(CustomIconsService.CustomIcon icon)
    {
      if (icon == null) return;

      string[] keys = null;
      if (CustomIcons.Icons != null)
        keys = CustomIcons.Icons.Select(x => x.Key).ToArray();

      var vm = await _navigation.NavigateAsync<SettingsIconsAddIconDialogModel>(new SettingsIconsAddIconDialogModel.SettingsIconsAddIconDialogModelParameter(keys, icon.Key, icon.PathData));
      if (await vm.DialogResult())
      {
        icon.Key = vm.Key;
        icon.PathData = vm.PathData;
        CustomIcons.Save();
      }
      
    }

    private async void DeleteCustomIcon(string key)
    {
      if (await _navigation.ShowQuestionAsync(Strings.TitleEditIconDialog, Strings.MessageQuestionDeleteCustomIcon))
      {
        CustomIcons.Delete(key);
        CustomIcons.Save();
      }
    }

    public RelayCommand ExportXamlCommand { get; set; }

    // TODO: Replace SaveFileDialog with INavigationService
    private void ExportXaml()
    {
      var sd = new SaveFileDialog();
      sd.AddExtension = true;
      sd.CheckPathExists = true;
      sd.DefaultExt = "*.xaml";
      sd.FileName = "UserResources.xaml";
      sd.DefaultExt = "*.xaml";
      sd.Filter = "Resource Dictionary File (.xaml)|*.xaml";
      sd.OverwritePrompt = true;
      var res = sd.ShowDialog();
      if (res.HasValue && res.Value)
      {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(
          "<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">");
        foreach (var icon in CustomIcons.Icons)
        {
          sb.AppendLine("    <Geometry x:Key=\"" + icon.Key + "\">" + icon.PathData + "</Geometry>");
        }
        sb.AppendLine("</ResourceDictionary>");
        File.WriteAllText(sd.FileName, sb.ToString());
      }
      
    }

  }
}