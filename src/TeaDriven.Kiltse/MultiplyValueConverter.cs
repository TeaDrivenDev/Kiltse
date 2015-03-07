using System;
using System.Globalization;

namespace TeaDriven.Kiltse
{
    public class MultiplyValueConverter : ConverterMarkupExtension<MultiplyValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}