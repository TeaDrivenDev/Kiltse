using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public static DependencyProperty ItemIndexProperty =
            DependencyProperty.Register("ItemIndex", typeof(int), typeof(Arc),
                new FrameworkPropertyMetadata(default(int),
                    FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));

        public static DependencyProperty TotalItemsProperty =
            DependencyProperty.Register("TotalItems", typeof(int), typeof(Arc),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender,
                    PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = dependencyObject as Arc;

            if (null != control)
            {
                control.Recalculate();
            }
        }

        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set { this.SetValue(RadiusProperty, value); }
        }

        public int ItemIndex
        {
            get { return (int)this.GetValue(ItemIndexProperty); }
            set { this.SetValue(ItemIndexProperty, value); }
        }

        public int TotalItems
        {
            get { return (int)this.GetValue(TotalItemsProperty); }
            set { this.SetValue(TotalItemsProperty, value); }
        }

        private void Recalculate()
        {
            const double gapPixels = 3;

            var gapHalfAngle = Maths.GapHalfAngle(gapPixels, Radius);
            var arcAngle = Maths.ArcAngle(this.TotalItems);
            var arcStartAngle = Maths.ArcStartAngle(ItemIndex, arcAngle, gapHalfAngle);
            var arcEndAngle = Maths.ArcEndAngle(ItemIndex, arcAngle, gapHalfAngle);

            var relativeStart = Maths.RelativePeripheralCoordinates(Radius, arcStartAngle);
            var relativeEnd = Maths.RelativePeripheralCoordinates(Radius, arcEndAngle);

            var startPoint = Maths.AbsolutePoint(Radius, relativeStart);
            var endPoint = Maths.AbsolutePoint(Radius, relativeEnd);

            Segment.Size = new Size(Radius, Radius);
            Segment.Point = startPoint;
            Segment.IsLargeArc = (1 == TotalItems);

            Figure.StartPoint = endPoint;
        }
    }
}