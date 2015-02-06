using GalaSoft.MvvmLight;

namespace WinHAB.Core.Mvvm
{
  public class WaiterViewModel : ViewModelBase
  {
    bool _isVisible;
    public bool IsVisible
    {
      get { return _isVisible; }
      set { _isVisible = value; RaisePropertyChanged(() => IsVisible); }
    }

    string _text;
    public string Text
    {
      get { return _text; }
      set { _text = value; RaisePropertyChanged(() => Text); }
    }

    public void Show()
    {
      IsVisible = true;
    }

    public void Show(string text)
    {
      Text = text;
      Show();
    }

    public void Hide()
    {
      IsVisible = false;
      Text = null;
    }
  }
}