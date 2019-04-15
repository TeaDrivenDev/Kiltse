using System.Windows.Media;

namespace TeaDriven.Kiltse
{
    public class DefaultStrokeInfoSelector : StrokeInfoSelector
    {
        public override StrokeInfo GetStrokeInfo(RingItem value)
        {
            return new StrokeInfo(new SolidColorBrush(Colors.White), DefaultStrokeThickness, null);
        }
    }
}