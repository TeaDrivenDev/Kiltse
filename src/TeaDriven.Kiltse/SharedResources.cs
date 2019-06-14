using System.Windows;

namespace TeaDriven.Kiltse
{
    public static class SharedResources
    {
        public static ComponentResourceKey DefaultRingSegmentToolTipTemplateKey { get; } = 
            new ComponentResourceKey(typeof(Iris), "DefaultRingSegmentToolTipTemplate");
    }
}