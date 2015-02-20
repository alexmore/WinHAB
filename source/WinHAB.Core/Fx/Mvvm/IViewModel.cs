using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface IViewModel : ICleanup
  {
    Task InitializeAsync(object parameter);
  }
}