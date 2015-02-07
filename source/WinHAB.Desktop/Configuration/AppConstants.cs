using System;
using System.IO;

namespace WinHAB.Desktop.Configuration
{
  public static class AppConstants
  {
    public static string ConfigurationFolder
    {
      get
      {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WinHAB"); 
      }
    }

    public static string ConfigurationFile {
      get { return Path.Combine(ConfigurationFolder, "WinHAB.json"); }
    }
  }
}