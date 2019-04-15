namespace TeaDriven.Kiltse
{
    public abstract class StrokeInfoSelector
    {
        public double DefaultStrokeThickness { get; set; } = 2;

        public abstract StrokeInfo GetStrokeInfo(object value);
    }
}