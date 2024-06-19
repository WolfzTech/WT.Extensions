using System.Collections;
using System.Windows.Controls;
using System.Windows;

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
                        selectedItems.Remove(item);
                    }
                    foreach (var item in e.AddedItems)
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }

        private static void UpdateSelectedItems(ListView listView, IList selectedItems)
        {
            listView.SelectedItems.Clear();
            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    listView.SelectedItems.Add(item);
                }
            }
        }
    }

}
