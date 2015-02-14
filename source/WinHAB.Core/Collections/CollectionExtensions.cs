using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace WinHAB.Core.Collections
{
  public static class CollectionExtensions
  {
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
      if (source == null) return null;

      return new ObservableCollection<T>(source);
    }

    public async static Task<ObservableCollection<T>> ToObservableCollectionAsync<T>(this Task<IEnumerable<T>> source)
    {
      if (source == null) return null;

      return new ObservableCollection<T>(await source);
    }
  }
}