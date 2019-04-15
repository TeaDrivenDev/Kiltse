using System;
using System.Windows;
using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    public static class Maths
    {
        public static double GapHalfAngle(double gapPixels, double radius)
        {
            return 90 * gapPixels / (Math.PI * radius);
        }

        public static double ArcAngle(int totalItems)
        {
            return 360 / (double)totalItems;
        }

        public static double ArcStartAngle(int index, double arcAngle, double gapHalfAngle)
        {
            return index * arcAngle + gapHalfAngle;
        }

        public static double ArcEndAngle(int index, double arcAngle, double gapHalfAngle)
        {
            return (index + 1) * arcAngle - gapHalfAngle;
        }

        public static double ToPolar(double cartesianAngle)
        {
            return Math.PI * cartesianAngle / 180;
        }

        public static Tuple<double, double> RelativePeripheralCoordinates(double radius, double cartesianAngle)
        {
            var polarAngle = ToPolar(cartesianAngle);

            var relativeX = radius * Math.Cos(polarAngle);
            var relativeY = radius * Math.Sin(polarAngle);

            return Tuple.Create(relativeX, -relativeY);
        }

        public static Point AbsolutePoint(double radius, Tuple<double, double> relativeCoordinates)
        {
            return new Point(radius + relativeCoordinates.Item1, radius + relativeCoordinates.Item2);
        }

        public static double AdjustForDirection(SweepDirection direction, double startAngle, double angle)
        {
            var sign = (SweepDirection.Clockwise == direction ? -1 : 1);

            return (startAngle + sign * angle) % 360;
        }
    }
}