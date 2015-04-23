using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using WinHAB.Core.Fx;
using WinHAB.Core.Fx.Mvvm;
using WinHAB.Desktop.Localization;

namespace WinHAB.Desktop.ViewModels.Settings
{
  public class SettingsIconsAddIconDialogModel : ViewModel, IDialogViewModel, IDataErrorInfo
  {
    private readonly INavigationService _navigation;
    private string[] _existingKeys;
    readonly TaskCompletionSource<bool> _dialogResultTaskCompletionSource = new TaskCompletionSource<bool>();

    public SettingsIconsAddIconDialogModel(INavigationService navigation)
    {
      _navigation = navigation;
    }

    public class SettingsIconsAddIconDialogModelParameter
    {
      public SettingsIconsAddIconDialogModelParameter(string[] keys, string key, string pathData)
      {
        Keys = keys;
        Key = key;
        PathData = pathData;
      }

      public string[] Keys { get; set; }
      public string Key { get; set; }
      public string PathData { get; set; }
    }

    public override async Task InitializeAsync(object parameter)
    {
      if (parameter == null && (parameter as SettingsIconsAddIconDialogModelParameter) == null) throw new ArgumentException("SettingsIconsAddIconDialogModel.InitializeAsync parameter argument must be of SettingsIconsAddIconDialogModelParameter type and can not be null.");
      
      var settingsIconsAddIconDialogModelParameter = parameter as SettingsIconsAddIconDialogModelParameter;
      if (settingsIconsAddIconDialogModelParameter != null)
        _existingKeys = settingsIconsAddIconDialogModelParameter.Keys ?? new string[0];

      OkCommand = new RelayCommand(() => CloseDialog(true), () => this["Key"] == null && this["PathData"] == null);
      CancelCommand = new RelayCommand(() => CloseDialog(false));

      var iconsAddIconDialogModelParameter = parameter as SettingsIconsAddIconDialogModelParameter;
      if (iconsAddIconDialogModelParameter != null)
      {
        Key = iconsAddIconDialogModelParameter.Key;
        if (!Key.IsNullOrWhitespace())
          _existingKeys = _existingKeys.Where(x => x != Key).ToArray();
      }

      var addIconDialogModelParameter = parameter as SettingsIconsAddIconDialogModelParameter;
      if (addIconDialogModelParameter != null)
        PathData = addIconDialogModelParameter.PathData;
    }

    private string _Key;
    public string Key { get { return _Key; } set { _Key = value; RaisePropertyChanged(()=>Key); if (OkCommand != null) OkCommand.RaiseCanExecuteChanged(); }}

    private string _PathData;
    public string PathData { get { return _PathData; } set { _PathData = value; RaisePropertyChanged(() => PathData); if (OkCommand != null) OkCommand.RaiseCanExecuteChanged(); } }

    public RelayCommand OkCommand { get; set; }
    public RelayCommand CancelCommand { get; set; }
    
    public Task<bool> DialogResult()
    {
      return _dialogResultTaskCompletionSource.Task;
    }

    private void CloseDialog(bool result)
    {
      _navigation.GoBack();
      _dialogResultTaskCompletionSource.SetResult(result);
    }

    public string this[string columnName]
    {
      get
      {
        if (columnName == "Key")
        {
          if (Key.IsNullOrWhitespace()) return Strings.MessageErrorKeyCanNotBeEmpty;
          if (_existingKeys.IsNotNull() && _existingKeys.Any(x => x.ToLower().Trim() == Key.ToLower().Trim()))
            return Strings.MessageErrorKeyAlreadyExists;
        }

        if (columnName == "PathData")
        {
          if (PathData.IsNullOrWhitespace()) return Strings.MessageErrorPathDataCanNotBeEmpty;
          try
          {
            Geometry.Parse(PathData);
          }
          catch
          {
            return Strings.MessageErrorPathDataMustBeValid;
          }
          
        }

        return null;
      }
    }

    public string Error { get; private set; }
  }
}