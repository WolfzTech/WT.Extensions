using System.Collections;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Specialized;

namespace WT.UI.ControlExtensions
{
    public static class DataGridExtensions
    {
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems",
                                                typeof(IList),
                                                typeof(DataGridExtensions),
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
            if (d is DataGrid datagrid)
            {
                datagrid.SelectionChanged -= ListView_SelectionChanged;

                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    // 🔹 Listen for changes in the collection
                    newCollection.CollectionChanged += (s, args) =>
                    {
                        UpdateSelectedItems(datagrid, (IList)e.NewValue);
                    };
                }

                if (e.NewValue is IList newList)
                {
                    datagrid.SelectionChanged += ListView_SelectionChanged;
                    UpdateSelectedItems(datagrid, newList);
                }
            }
        }

        private static void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                IList selectedItems = GetSelectedItems(dataGrid);
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
                    UpdateSelectedItems(dataGrid, selectedItems);
                }
            }
        }

        private static void UpdateSelectedItems(DataGrid dataGrid, IList selectedItems)
        {
            dataGrid.SelectionChanged -= ListView_SelectionChanged;

            // 🔹 Clear selection and reapply the selected items
            dataGrid.SelectedItems.Clear();
            foreach (var item in selectedItems)
            {
                dataGrid.SelectedItems.Add(item);
            }

            dataGrid.SelectionChanged += ListView_SelectionChanged;
        }
    }

}
