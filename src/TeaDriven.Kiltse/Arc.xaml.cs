using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    /// <summary>
    /// Interaction logic for Arc.xaml
    /// </summary>
    public partial class Arc : UserControl
    {
        public Arc()
        {
            InitializeComponent();
        }

        public static DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(default(double),
                    FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));

        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value); }
        }

        public static DependencyProperty ItemIndexProperty =
             DependencyProperty.Register("ItemIndex", typeof(int), typeof(Arc),
                 new FrameworkPropertyMetadata(default(int),
                     FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));

        public int ItemIndex
        {
            get { return (int)this.GetValue(ItemIndexProperty); }
            set { this.SetValue(ItemIndexProperty, value); }
        }

        public static DependencyProperty TotalItemsProperty =
            DependencyProperty.Register("TotalItems", typeof(int), typeof(Arc),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        public int TotalItems
        {
            get { return (int)this.GetValue(TotalItemsProperty); }
            set { this.SetValue(TotalItemsProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as Arc;

            if (null != control)
            {
                control.Recalculate();
            }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(SweepDirection), typeof(Arc),
            new FrameworkPropertyMetadata(SweepDirection.Clockwise,
                FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));

        public SweepDirection Direction
        {
            get { return (SweepDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(90d, FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        //public static readonly DependencyProperty StrokeThicknessProperty =
        //    DependencyProperty.Register("StrokeThickness", typeof(double), typeof(Ring),
        //        new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.AffectsRender));

        //public double StrokeThickness
        //{
        //    get { return (double)GetValue(StrokeThicknessProperty); }
        //    set { SetValue(StrokeThicknessProperty, value); }
        //}

        //public static readonly DependencyProperty HighlightStrokeThicknessProperty =
        //    DependencyProperty.Register("HighlightStrokeThickness", typeof(double), typeof(Ring),
        //        new PropertyMetadata(4d));

        //public double HighlightStrokeThickness
        //{
        //    get { return (double)GetValue(HighlightStrokeThicknessProperty); }
        //    set { SetValue(HighlightStrokeThicknessProperty, value); }
        //}

        private void Recalculate()
        {
            const double gapPixels = 3;

            var gapHalfAngle = Maths.GapHalfAngle(gapPixels, Radius);
            var arcAngle = Maths.ArcAngle(TotalItems);

            var arcStartAngle =
                Maths.AdjustForDirection(Direction, StartAngle,
                    Maths.ArcStartAngle(ItemIndex, arcAngle, gapHalfAngle));
            var arcEndAngle =
                Maths.AdjustForDirection(Direction, StartAngle,
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