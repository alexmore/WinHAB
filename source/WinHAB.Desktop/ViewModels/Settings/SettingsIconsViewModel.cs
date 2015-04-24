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
    public SettingsIconsViewModel()
    {
      Title = Strings.TitleDefaultIcons;
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
    }

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(()=>Title); }}
    
    private ObservableCollection<KeyValuePair<string, Geometry>> _DefaultIcons;
    public ObservableCollection<KeyValuePair<string, Geometry>> DefaultIcons { get { return _DefaultIcons; } set { _DefaultIcons = value; RaisePropertyChanged(() => DefaultIcons); } }
  }
}