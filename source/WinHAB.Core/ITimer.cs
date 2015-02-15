using System;

namespace WinHAB.Core
{
  public interface ITimer
  {
    TimeSpan Interval { get; set; }
    bool IsEnabled { get; set; }

    void Start();
    void Stop();

    event EventHandler Tick;
  }
}