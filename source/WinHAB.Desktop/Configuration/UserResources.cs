﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Resources;
using WinHAB.Core;
using WinHAB.Desktop.Windows;

namespace WinHAB.Desktop.Configuration
{
  public static class UserResources
  {
    public static ResourceDictionary Icons { get; private set; }

    public static async Task LoadUserResources(string serverAddress, OpenHabClient client)
    {
      Icons = new ResourceDictionary();

      Icons.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("/Themes/WidgetIcons.xaml", UriKind.Relative) });

      try
      {
        var stream = await client.GetResource(new Uri(serverAddress + "/winhab/UserResources.xaml"));
        var userResourcesReader = new System.Windows.Markup.XamlReader();
        Icons.MergedDictionaries.Add(LoadReasourceDictionary(stream));
      }
      catch
      {
      }
    }

    public static ResourceDictionary LoadReasourceDictionary(Stream stream)
    {
      try
      {
        var reader = new System.Windows.Markup.XamlReader();
        return (ResourceDictionary) reader.LoadAsync(stream);
      }
      catch
      {
        
      }

      return null;
    }
  }
}