using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    [TemplatePart(Name = PART_DisplayName, Type = typeof(TextBlock))]
    public class Iris : ContentControl
    {
        private ContentControl displayNameContentControl;

        public const string PART_DisplayName = "PART_DisplayName";

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

        public static readonly DependencyProperty GapWidthProperty =
            DependencyProperty.Register(
                nameof(GapWidth),
                typeof(double),
                typeof(Iris),
                new PropertyMetadata(Constants.DefaultGapWidth));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                nameof(StartAngle),
                typeof(double),
                typeof(Iris),
                new PropertyMetadata(Constants.DefaultStartAngle));

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
                new PropertyMetadata(new DefaultStrokeInfoSelector()));

        public static readonly DependencyProperty DisplayNameTemplateProperty =
            DependencyProperty.Register(
                nameof(DisplayNameTemplate),
                typeof(ControlTemplate),
                typeof(Iris),
                new PropertyMetadata(default(ControlTemplate)));

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

        public double GapWidth
        {
            get => (double)GetValue(GapWidthProperty);
            set => SetValue(GapWidthProperty, value);
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

        public ControlTemplate DisplayNameTemplate
        {
            get => (ControlTemplate)GetValue(DisplayNameTemplateProperty);
            set => SetValue(DisplayNameTemplateProperty, value);
        }

        public double DisplayNameAreaSize => Math.Sqrt(2 * this.Radius * this.Radius);

        public ObservableCollection<RingItem> Items { get; } = new ObservableCollection<RingItem>();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var displayNameControl =
                this.displayNameContentControl
                ?? (this.displayNameContentControl =
                    GetTemplateChild(PART_DisplayName) as ContentControl);

            if (null != displayNameControl)
            {
                if (null != this.DisplayNameTemplate)
                {
                    displayNameControl.Template = this.DisplayNameTemplate;
                }
                else
                {
                    this.DisplayNameTemplate = displayNameControl.Template;
                }
            }
        }

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