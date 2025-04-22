using System.Collections;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Specialized;

namespace WT.UI.ControlExtensions
{
    public static class ListViewExtensions
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems",
                                                typeof(IList),
                                                typeof(ListViewExtensions),
                                                new PropertyMetadata(null, OnSelectedItemsChanged));

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                listView.SelectionChanged -= ListView_SelectionChanged;

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    // 🔹 Listen for changes in the collection
                    newCollection.CollectionChanged += (s, args) =>
                    {
                        UpdateSelectedItems(listView, (IList)e.NewValue);
                    };
                }

                if (e.NewValue is IList newList)
                {
                    listView.SelectionChanged += ListView_SelectionChanged;
                    UpdateSelectedItems(listView, newList);
                }
            }
        }

        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView)
            {
                IList selectedItems = GetSelectedItems(listView);
                if (selectedItems != null)
                {
                    foreach (var item in e.RemovedItems)
                    {
                        if (selectedItems.Contains(item))
                        {
                            selectedItems.Remove(item);
                        }
                    }

                    foreach (var item in e.AddedItems)
                    {
                        if (!selectedItems.Contains(item))
                        {
                            selectedItems.Add(item);
                        }
                    }

                    // 🔹 Force ListView to refresh selections
                    UpdateSelectedItems(listView, selectedItems);
                }
            }
        }

        private static void UpdateSelectedItems(ListView listView, IList selectedItems)
        {
            listView.SelectionChanged -= ListView_SelectionChanged;

            // 🔹 Clear selection and reapply the selected items
            listView.SelectedItems.Clear();
            foreach (var item in selectedItems)
            {
                listView.SelectedItems.Add(item);
            }

            listView.SelectionChanged += ListView_SelectionChanged;
        }
    }

}
