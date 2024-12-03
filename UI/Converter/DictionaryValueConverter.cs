using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WT.UI.Converter
{
    public class DictionaryValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<string, string> dictionary && parameter is string key)
            {
                if (dictionary.TryGetValue(key, out var dictValue))
                {
                    return dictValue;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
