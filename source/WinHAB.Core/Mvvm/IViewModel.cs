using GalaSoft.MvvmLight;

namespace WinHAB.Core.Mvvm
{
  public interface IViewModel : ICleanup
  {
    INavigationService Navigation { get; }
  }
}