using System;

namespace WinHAB.Core.Fx.Mvvm
{
  [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
  public sealed class SingletonViewModelAttribute : Attribute
  {
  }
}