using GalaSoft.MvvmLight;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface IViewModel : ICleanup
  {
    INavigationService Navigation { get; }

    void OnLoaded();
  }
}