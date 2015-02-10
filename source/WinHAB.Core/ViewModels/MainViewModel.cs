using WinHAB.Core.Configuration;
using WinHAB.Core.Mvvm;

namespace WinHAB.Core.ViewModels
{
  public class MainViewModel : ViewModel
  {
    public MainViewModel(INavigationService navigationService, AppConfiguration appConfig) : base(navigationService)
    {
      Title = "Demo House";
      AppConfiguration = appConfig;
    }

    public AppConfiguration AppConfiguration { get; private set; }
    public INavigationService NavigationService { get { return Navigation; }}

    private string _Title;
    public string Title { get { return _Title; } set { _Title = value; RaisePropertyChanged(() => Title); } }
  }
}