using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TeaDriven.Kiltse
{
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter,
        IMultiValueConverter
        where T : class, new()
    {
        private static T _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new T();
            }

            return _converter;
        }

        #region IValueConverter Members

        public virtual object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IValueConverter Members

        #region IMultiValueConverter Members

        public virtual object Convert(object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion IMultiValueConverter Members
    }
}