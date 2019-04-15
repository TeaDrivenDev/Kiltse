using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    public class Iris : ContentControl
    {
        static Iris()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Iris),
                new FrameworkPropertyMetadata(typeof(Iris)));
        }

        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register(
                nameof(DisplayName),
                typeof(string),
                typeof(Iris),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                nameof(Radius),
                typeof(double),
                typeof(Iris),
                new FrameworkPropertyMetadata(
                    default(double),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                nameof(StartAngle),
                typeof(double),
                typeof(Iris),
                new PropertyMetadata(90d));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(SweepDirection),
                typeof(Iris),
                new PropertyMetadata(SweepDirection.Clockwise));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                nameof(ItemsSource),
                typeof(IEnumerable<object>),
                typeof(Iris),
                new PropertyMetadata(Enumerable.Empty<object>(), ItemsChangedCallback));

        public static readonly DependencyProperty StrokeInfoSelectorProperty =
            DependencyProperty.Register(
                nameof(StrokeInfoSelector),
                typeof(StrokeInfoSelector),
                typeof(Iris),
                new PropertyMetadata(default(StrokeInfoSelector)));

        public string DisplayName
        {
            get => (string)GetValue(DisplayNameProperty);
            set => SetValue(DisplayNameProperty, value);
        }

        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public double StartAngle
        {
            get => (double)GetValue(StartAngleProperty);
            set => SetValue(StartAngleProperty, value);
        }

        public SweepDirection Direction
        {
            get => (SweepDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public StrokeInfoSelector StrokeInfoSelector
        {
            get => (StrokeInfoSelector)GetValue(StrokeInfoSelectorProperty);
            set => SetValue(StrokeInfoSelectorProperty, value);
        }

        public ObservableCollection<RingItem> Items { get; } = new ObservableCollection<RingItem>();

        private static void ItemsChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is Iris control)
            {
                var list =
                    ((IEnumerable<object>)dependencyPropertyChangedEventArgs.NewValue).ToList();

                var ringItems = list.Select((item, index) => new RingItem(index, item));

                control.Items.Clear();
                foreach (var item in ringItems)
                {
                    control.Items.Add(item);
                }
            }
        }
    }
}