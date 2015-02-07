namespace WinHAB.Core.Mvvm
{
  public interface IViewModelViewMapper
  {
    IViewModelViewMapper Map<TViewModel, TView>()
      where TViewModel : IViewModel
      where TView : IView; 
  }
}