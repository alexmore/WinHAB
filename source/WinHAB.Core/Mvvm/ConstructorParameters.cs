﻿using System;
using System.Collections.Generic;

namespace WinHAB.Core.Mvvm
{
  public class ConstructorParameters
  {
    public ConstructorParameters()
    {
      Parameters = new List<KeyValuePair<string, object>>();
    }

    public List<KeyValuePair<string, object>> Parameters { get; private set; }

    public ConstructorParameters Add(string name, object value)
    {
      Parameters.Add(new KeyValuePair<string, object>(name, value));
      return this;
    }

    public static ConstructorParameters GetConstructorParameters(Action<ConstructorParameters> action)
    {
      if (action == null) return null;
      
      var ps = new ConstructorParameters();
      action(ps);
      return ps;
    }
  }
}