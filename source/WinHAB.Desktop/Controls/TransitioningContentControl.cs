using System.Windows;

namespace WinHAB.Desktop.Controls
{
  public class TransitioningContentControl : FirstFloor.ModernUI.Windows.Controls.TransitioningContentControl
  {
    public void StartTransition()
    {
      OnContentChanged(null, this.Content);
    }
  }
}