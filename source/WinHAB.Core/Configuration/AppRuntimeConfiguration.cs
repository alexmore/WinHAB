namespace WinHAB.Core.Configuration
{
  public class AppRuntimeConfiguration
  {
    public AppRuntimeConfiguration()
    {
      IsRestarting = false;
    }

    public bool IsRestarting { get; set; }
  }
}