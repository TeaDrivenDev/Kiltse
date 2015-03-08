using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Ring),
                new PropertyMetadata(default(SweepDirection)));

        public SweepDirection Direction
        {
            get { return (SweepDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Ring),
                new PropertyMetadata(90d));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Ring),
                new PropertyMetadata(2d));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty HighlightStrokeThicknessProperty =
            DependencyProperty.Register("HighlightStrokeThickness", typeof(double), typeof(Ring),
                new PropertyMetadata(4d));

        public double HighlightStrokeThickness
        {
            get { return (double)GetValue(HighlightStrokeThicknessProperty); }
            set { SetValue(HighlightStrokeThicknessProperty, value); }
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