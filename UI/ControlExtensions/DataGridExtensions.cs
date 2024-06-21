using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

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
            if (d is DataGrid dataGrid)
            {
                dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
                if (e.NewValue is IList newList)
                {
                    dataGrid.SelectionChanged += DataGrid_SelectionChanged;
                    UpdateSelectedItems(dataGrid, newList);
                }
            }
        }

        private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                IList selectedItems = GetSelectedItems(dataGrid);
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

        private static void UpdateSelectedItems(DataGrid dataGrid, IList selectedItems)
        {
            dataGrid.SelectedItems.Clear();
            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    dataGrid.SelectedItems.Add(item);
                }
            }
        }
    }

}
