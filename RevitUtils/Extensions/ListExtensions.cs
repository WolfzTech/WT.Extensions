using System.Collections.ObjectModel;

namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> source)
        {
            ObservableCollection<T> obs = new ObservableCollection<T>();
            foreach (T member in source)
            {
                obs.Add(member);
            }
            return obs;
        }
    }
}
