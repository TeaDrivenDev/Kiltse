using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    public class Iris : ContentControl
    {
        static Iris()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Iris), new FrameworkPropertyMetadata(typeof(Iris)));
        }

        public static readonly DependencyProperty DisplayNameProperty =
      DependencyProperty.Register("DisplayName", typeof(string), typeof(Iris),
          new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Iris),
                new FrameworkPropertyMetadata(default(double),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(Iris),
                new PropertyMetadata(new object[] { }, ItemsChangedCallback));

        private static void ItemsChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as Iris;

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
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Iris),
                new PropertyMetadata(SweepDirection.Clockwise));

        public SweepDirection Direction
        {
            get { return (SweepDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Iris),
                new PropertyMetadata(90d));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Iris),
                new PropertyMetadata(2d));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty HighlightStrokeThicknessProperty =
            DependencyProperty.Register("HighlightStrokeThickness", typeof(double), typeof(Iris),
                new PropertyMetadata(4d));

        public double HighlightStrokeThickness
        {
            get { return (double)GetValue(HighlightStrokeThicknessProperty); }
            set { SetValue(HighlightStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(Iris),
                new PropertyMetadata(Brushes.DarkSlateGray));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static RoutedEvent RingSegmentClickEvent =
            EventManager.RegisterRoutedEvent("RingSegmentClick", RoutingStrategy.Bubble,
                typeof(RoutedEvent), typeof(Iris));

        public event RoutedEventHandler RingSegmentClick
        {
            add { this.AddHandler(RingSegmentClickEvent, value); }

            remove { this.RemoveHandler(RingSegmentClickEvent, value); }
        }

        public void Arc_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var arc = sender as Arc;

            if (null != arc)
            {
                var ringItem = (RingItem)arc.DataContext;

                RaiseEvent(new RingSegmentClickEventArgs(RingSegmentClickEvent, ringItem));
            }
        }
    }
}