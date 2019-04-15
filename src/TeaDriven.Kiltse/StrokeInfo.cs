using System.Windows.Media;
using System.Windows.Media.Effects;

namespace TeaDriven.Kiltse
{
    public class StrokeInfo
    {
        public StrokeInfo(Brush stroke, double strokeThickness, Effect effect)
        {
            Stroke = stroke;
            StrokeThickness = strokeThickness;
            Effect = effect;
        }

        public Brush Stroke { get; }

        public double StrokeThickness { get; }

        public Effect Effect { get; }
    }
}