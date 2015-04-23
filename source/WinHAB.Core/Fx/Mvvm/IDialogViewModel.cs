using System.Threading.Tasks;

namespace WinHAB.Core.Fx.Mvvm
{
  public interface IDialogViewModel
  {
    Task<bool> DialogResult();
  }
}