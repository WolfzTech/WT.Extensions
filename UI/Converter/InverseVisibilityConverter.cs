﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WT.UI.Converter
{
    public class InverseVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
            throw new ArgumentException("Value is not a Visibility");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
            throw new ArgumentException("Value is not a Visibility");
        }
    }
}
