using System;
using System.Windows.Data;

namespace WT.Revit.UI.Converter
{
    public class BoolInverse : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool val)
            {
                return !val;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is bool val)
            {
                return !val;
            }
            return value;
        }
    }
}
