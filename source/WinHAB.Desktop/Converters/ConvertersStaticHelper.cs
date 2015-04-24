using Ninject;
using WinHAB.Core.Configuration;

namespace WinHAB.Desktop.Converters
{
  public static class ConvertersStaticHelper
  {
    public static IKernel Kernel { get; set; }

    public static UserResources UserResources { get { return Kernel.Get<AppConfiguration>().UserResources; } }
  }
}