namespace TeaDriven.Kiltse
{
    public abstract class StrokeInfoSelector
    {
        public double DefaultStrokeThickness { get; set; } = Constants.DefaultStrokeThickness;

        public abstract StrokeInfo GetStrokeInfo(RingItem value);
    }
}