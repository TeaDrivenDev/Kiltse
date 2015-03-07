using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeaDriven.Kiltse
{
    /// <summary>
    /// Interaction logic for Ring.xaml
    /// </summary>
    public partial class Ring : UserControl
    {
        public Ring()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(Ring),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Ring),
                new FrameworkPropertyMetadata(default(double),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(Ring),
                new PropertyMetadata(new object[] { }, ItemsChangedCallback));

        private static void ItemsChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as Ring;

            if (null != control)
            {
                var list =
                    ((IEnumerable<object>)dependencyPropertyChangedEventArgs.NewValue).ToList();
                var count = list.Count;

                var ringItems = list.Select((item, index) => new RingItem(index, count, item));

                control._items.Clear();
                foreach (var item in ringItems)
                {
                    control._items.Add(item);
                }
            }
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private readonly ObservableCollection<RingItem> _items =
            new ObservableCollection<RingItem>();

        public ObservableCollection<RingItem> Items
        {
            get { return this._items; }
        }
    }

    public class RingItem
    {
        public int ItemIndex { get; private set; }

        public int TotalItems { get; private set; }

        public object Item { get; private set; }

        public RingItem(int itemIndex, int totalItems, object item)
        {
            ItemIndex = itemIndex;
            TotalItems = totalItems;
            Item = item;
        }
    }
}