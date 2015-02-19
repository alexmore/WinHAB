using System;

namespace WinHAB.Core.Fx.Mvvm
{
  [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
  public sealed class ViewModelAttribute : Attribute
  {
    public ViewModelAttribute(Type viewModelType)
    {
      ViewModelType = viewModelType;
    }

    public Type ViewModelType { get; private set; }
  }
}