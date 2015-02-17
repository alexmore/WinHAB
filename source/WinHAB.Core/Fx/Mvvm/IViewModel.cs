using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface IViewModel : ICleanup
  {
    INavigationService Navigation { get; }

    Task InitializeAsync(dynamic parameter);
  }
}