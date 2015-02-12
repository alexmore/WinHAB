using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WinHAB.Core.Collections
{
  public static class CollectionExtensions
  {
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
      if (source == null) return null;

      return new ObservableCollection<T>(source);
    }
  }
}