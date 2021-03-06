﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Helpers;

namespace WinHAB.Core.Fx.Mvvm
{
  public class AsyncRelayCommand<T> : ICommand
  {
    private readonly WeakFunc<T, Task> _execute;
    private readonly WeakFunc<T, bool> _canExecute;

    /// <summary>
    /// Occurs when changes occur that affect whether the command should execute.
    /// 
    /// </summary>
    public event EventHandler CanExecuteChanged;

    /// <summary>
    /// Initializes a new instance of the RelayCommand class that
    ///             can always execute.
    /// 
    /// </summary>
    /// <param name="execute">The execution logic.</param><exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
    public AsyncRelayCommand(Func<T, Task> execute)
      : this(execute, (Func<T, bool>) null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the RelayCommand class.
    /// 
    /// </summary>
    /// <param name="execute">The execution logic.</param><param name="canExecute">The execution status logic.</param><exception cref="T:System.ArgumentNullException">If the execute argument is null.</exception>
    public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException("execute");
      this._execute = new WeakFunc<T, Task>(execute);
      if (canExecute == null)
        return;
      this._canExecute = new WeakFunc<T, bool>(canExecute);
    }

    /// <summary>
    /// Raises the <see cref="E:GalaSoft.MvvmLight.Command.RelayCommand`1.CanExecuteChanged"/> event.
    /// 
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
      EventHandler eventHandler = this.CanExecuteChanged;
      if (eventHandler == null)
        return;
      eventHandler((object) this, EventArgs.Empty);
    }

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// 
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require data
    ///             to be passed, this object can be set to a null reference</param>
    /// <returns>
    /// true if this command can be executed; otherwise, false.
    /// </returns>
    public bool CanExecute(object parameter)
    {
      if (this._canExecute == null)
        return true;
      if (this._canExecute.IsStatic || this._canExecute.IsAlive)
      {
        if (parameter == null && IntrospectionExtensions.GetTypeInfo(typeof (T)).IsValueType)
          return this._canExecute.Execute(default (T));
        if (parameter is T)
          return this._canExecute.Execute((T) parameter);
      }
      return false;
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// 
    /// </summary>
    /// <param name="parameter">Data used by the command. If the command does not require data
    ///             to be passed, this object can be set to a null reference</param>
    public virtual async void Execute(object parameter)
    {
      await ExecuteAsync(parameter);
    }

    public virtual Task ExecuteAsync(object parameter)
    {
      object parameter1 = parameter;
      if (!this.CanExecute(parameter1) || this._execute == null || !this._execute.IsStatic && !this._execute.IsAlive)
        return Task.FromResult(0);
      if (parameter1 == null)
      {
        if (IntrospectionExtensions.GetTypeInfo(typeof(T)).IsValueType)
          return this._execute.Execute(default(T));
        else
          return this._execute.Execute((T)parameter1);
      }
      else
        return this._execute.Execute((T)parameter1);
    }
  }
}