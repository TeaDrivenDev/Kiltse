using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Ring),
                new FrameworkPropertyMetadata(default(double),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(Ring),
                new PropertyMetadata(new object[] { }, ItemsChangedCallback));

        private static void ItemsChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is Ring control)
            {
                var list =
                    ((IEnumerable<object>)dependencyPropertyChangedEventArgs.NewValue).ToList();

                var ringItems = list.Select((item, index) => new RingItem(index, item));

                control._items.Clear();
                foreach (var item in ringItems)
                {
                    control._items.Add(item);
                }
            }
        }

        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private readonly ObservableCollection<RingItem> _items =
            new ObservableCollection<RingItem>();

        public ObservableCollection<RingItem> Items => this._items;

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Ring),
                new PropertyMetadata(SweepDirection.Clockwise));

        public SweepDirection Direction
        {
            get => (SweepDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Ring),
                new PropertyMetadata(90d));

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Ring),
                new PropertyMetadata(2d));

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty HighlightStrokeThicknessProperty =
            DependencyProperty.Register("HighlightStrokeThickness", typeof(double), typeof(Ring),
                new PropertyMetadata(4d));

        public double HighlightStrokeThickness
        {
            get => (double)GetValue(HighlightStrokeThicknessProperty);
            set => SetValue(HighlightStrokeThicknessProperty, value);
        }

        public static RoutedEvent RingSegmentClickEvent =
            EventManager.RegisterRoutedEvent("RingSegmentClick", RoutingStrategy.Bubble,
                typeof(RoutedEvent), typeof(Ring));

        public event RoutedEventHandler RingSegmentClick
        {
            add => this.AddHandler(RingSegmentClickEvent, value);

            remove => this.RemoveHandler(RingSegmentClickEvent, value);
        }

        private void Arc_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Arc arc)
            {
                var ringItem = (RingItem)arc.DataContext;

                RaiseEvent(new RingSegmentClickEventArgs(RingSegmentClickEvent, ringItem));
            }
        }
    }

    public class RingItem
    {
        public int ItemIndex { get; }

        public object Item { get; }

        public string Name => this.Item.ToString();

        public RingItem(int itemIndex, object item)
        {
            ItemIndex = itemIndex;
            Item = item;
        }
    }
}