using System.Windows;
using System.Windows.Controls;

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

        public static readonly DependencyProperty SpinDirectionProperty =
            DependencyProperty.Register("SpinDirection", typeof(SpinDirection), typeof(Arc),
            new FrameworkPropertyMetadata(SpinDirection.Clockwise,
                FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));

        public SpinDirection SpinDirection
        {
            get { return (SpinDirection)GetValue(SpinDirectionProperty); }
            set { SetValue(SpinDirectionProperty, value); }
        }

        private void Recalculate()
        {
            const double gapPixels = 3;

            var gapHalfAngle = Maths.GapHalfAngle(gapPixels, Radius);
            var arcAngle = Maths.ArcAngle(TotalItems);

            var arcStartAngle =
                Maths.AdjustForDirection(SpinDirection, 90,
                    Maths.ArcStartAngle(ItemIndex, arcAngle, gapHalfAngle));
            var arcEndAngle =
                Maths.AdjustForDirection(SpinDirection, 90,
                    Maths.ArcEndAngle(ItemIndex, arcAngle, gapHalfAngle));

            var relativeStart =
                (SpinDirection.Clockwise == SpinDirection
                    ? Maths.RelativePeripheralCoordinates(Radius, arcStartAngle)
                    : Maths.RelativePeripheralCoordinates(Radius, arcEndAngle));
            var relativeEnd =
                (SpinDirection.Clockwise == SpinDirection
                    ? Maths.RelativePeripheralCoordinates(Radius, arcEndAngle)
                    : Maths.RelativePeripheralCoordinates(Radius, arcStartAngle));

            var startPoint = Maths.AbsolutePoint(Radius, relativeStart);
            var endPoint = Maths.AbsolutePoint(Radius, relativeEnd);

            Segment.Size = new Size(Radius, Radius);
            Segment.Point = startPoint;
            Segment.IsLargeArc = (1 == TotalItems);

            Figure.StartPoint = endPoint;
        }
    }
}