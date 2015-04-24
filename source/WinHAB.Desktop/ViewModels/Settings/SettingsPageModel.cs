using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Core.ViewModels;
using WinHAB.Desktop.Configuration;

namespace WinHAB.Desktop.ViewModels.Settings
{
  [SingletonViewModel]
  public class SettingsPageModel : PageModelBase
  {
    public SettingsPageModel(INavigationService navigationService, DesktopConfiguration config) : base(navigationService)
    {
      BackCommand = new RelayCommand(Navigation.GoBack);

      ViewModels = new ObservableCollection<ITitledViewModel>();
      ViewModels.Add(new SettingsConnectionViewModel(Navigation, config));
      ViewModels.Add(new SettingsAppearanceViewModel(Navigation, config));
      ViewModels.Add(new SettingsIconsViewModel(Navigation, config));
    }

    public override async Task InitializeAsync(object parameter)
    {
      var viewModel = ViewModels.FirstOrDefault();
      if (parameter != null)
        viewModel = ViewModels.FirstOrDefault(x => x.GetType() == (Type) parameter) ?? viewModel;

      SelectedViewModel = viewModel;
    }
    
    public RelayCommand BackCommand { get; set; }

    private ObservableCollection<ITitledViewModel> _ViewModels;
    public ObservableCollection<ITitledViewModel> ViewModels { get { return _ViewModels; } set { _ViewModels = value; RaisePropertyChanged(()=>ViewModels); }}

    private ITitledViewModel _SelectedViewModel;
    public ITitledViewModel SelectedViewModel { get { return _SelectedViewModel; } set { _SelectedViewModel = value; RaisePropertyChanged(() => SelectedViewModel); } }
  }
}
