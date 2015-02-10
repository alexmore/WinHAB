using WinHAB.Core;
using WinHAB.Core.Configuration;

namespace WinHAB.Desktop.Configuration
{
  public class DesktopConfiguration : AppConfiguration
  {
    public DesktopConfiguration(IConfigurationProvider provider) : base(provider)
    {
    }

    #region Appearance

    public string AccentColor
    {
      get { return Provider.Get(this.GetPropertyName(() => AccentColor)); }
      set { Provider.Set(this.GetPropertyName(() => AccentColor), value); }
    }

    public string ThemeSource
    {
      get { return Provider.Get(this.GetPropertyName(() => ThemeSource)); }
      set { Provider.Set(this.GetPropertyName(() => ThemeSource), value); }
    }

    #endregion
  }
}