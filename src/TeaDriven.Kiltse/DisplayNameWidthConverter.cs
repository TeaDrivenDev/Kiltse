using System;
using System.Globalization;

namespace TeaDriven.Kiltse
{
    public class DisplayNameWidthConverter : ConverterMarkupExtension<DisplayNameWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var radius = (double)value;

            return 2 * radius - 10;
        }
    }
}