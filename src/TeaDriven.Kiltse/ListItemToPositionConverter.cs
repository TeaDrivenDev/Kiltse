using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    // source: http://stackoverflow.com/questions/3607368/wpf-binding-lists-in-xaml-how-can-an-item-know-its-position-in-the-list
    public class ListItemToPositionConverter : ConverterMarkupExtension<ListItemToPositionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as ListBoxItem;
            if (item != null)
            {
                var lb = FindAncestor<ListBox>(item);
                if (lb != null)
                {
                    var index = lb.Items.IndexOf(item.Content);
                    return index;
                }
            }
            return null;
        }

        // TODO: This does not quite belong here.
        public static T FindAncestor<T>(DependencyObject from) where T : class
        {
            if (from == null)
                return null;

            var candidate = from as T;
            return candidate ?? FindAncestor<T>(VisualTreeHelper.GetParent(from));
        }
    }

    public class PositionToColorConverter : ConverterMarkupExtension<PositionToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = (int)value;

            switch (index)
            {
                case 0:
                    return Brushes.Red;

                case 1:
                    return Brushes.Cyan;

                default:
                    return Brushes.AntiqueWhite;
            }
        }
    }

    public class DisplayNameWidthConverter : ConverterMarkupExtension<DisplayNameWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var radius = (double)value;

            return 2 * radius - 10;
        }
    }
}