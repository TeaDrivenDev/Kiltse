using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace TeaDriven.Kiltse
{
    public partial class Arc
    {
        public Arc()
        {
            InitializeComponent();
        }

        public static DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                nameof(Radius),
                typeof(double),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    default(double),
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        public static readonly DependencyProperty GapWidthProperty =
            DependencyProperty.Register(
                nameof(GapWidth),
                typeof(double),
                typeof(Arc),
                new PropertyMetadata(Constants.DefaultGapWidth, PropertyChangedCallback));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                nameof(StartAngle),
                typeof(double),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    Constants.DefaultStartAngle,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(SweepDirection),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    SweepDirection.Clockwise,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback,
                    CoerceValueCallback));

        public static DependencyProperty ItemIndexProperty =
            DependencyProperty.Register(
                nameof(ItemIndex),
                typeof(int),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    default(int),
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        public static DependencyProperty TotalItemsProperty =
            DependencyProperty.Register(
                nameof(TotalItems),
                typeof(int),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    1,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                nameof(Stroke),
                typeof(Brush),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    default(Brush),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(double),
                typeof(Arc),
                new FrameworkPropertyMetadata(
                    Constants.DefaultStrokeThickness,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeEffectProperty =
            DependencyProperty.Register(
                nameof(StrokeEffect),
                typeof(Effect),
                typeof(Arc),
                new UIPropertyMetadata(default(Effect)));

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

        public int ItemIndex
        {
            get => (int)GetValue(ItemIndexProperty);
            set => SetValue(ItemIndexProperty, value);
        }

        public int TotalItems
        {
            get => (int)GetValue(TotalItemsProperty);
            set => SetValue(TotalItemsProperty, value);
        }

        public Brush Stroke
        {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public Effect StrokeEffect
        {
            get => (Effect)GetValue(StrokeEffectProperty);
            set => SetValue(StrokeEffectProperty, value);
        }

        private static void PropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject is Arc control)
            {
                control.Recalculate();
            }
        }

        private static object CoerceValueCallback(DependencyObject d, object basevalue)
        {
            if (d is Arc control)
            {
                control.Recalculate();
            }

            return basevalue;
        }

        private void Recalculate()
        {
            var gapHalfAngle = Maths.GapHalfAngle(GapWidth, Radius);
            var arcAngle = Maths.ArcAngle(TotalItems);

            var arcStartAngle =
                Maths.AdjustForDirection(
                    Direction,
                    StartAngle,
                    Maths.ArcStartAngle(ItemIndex, arcAngle, gapHalfAngle));
            var arcEndAngle =
                Maths.AdjustForDirection(
                    Direction,
                    StartAngle,
                    Maths.ArcEndAngle(ItemIndex, arcAngle, gapHalfAngle));

            var relativeStart = Maths.RelativePeripheralCoordinates(Radius, arcEndAngle);
            var relativeEnd = Maths.RelativePeripheralCoordinates(Radius, arcStartAngle);

            var startPoint = Maths.AbsolutePoint(Radius, relativeStart);
            var endPoint = Maths.AbsolutePoint(Radius, relativeEnd);

            Segment.Size = new Size(Radius, Radius);
            Segment.Point = startPoint;
            Segment.IsLargeArc = (1 == TotalItems);

            Figure.StartPoint = endPoint;
        }
    }
}