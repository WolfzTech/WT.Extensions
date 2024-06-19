using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WT.UI.Converter
{
    public class BoolToVisibility : BooleanConverter<Visibility>
    {
        public BoolToVisibility() :
          base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
