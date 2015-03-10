using System.Windows;

namespace TeaDriven.Kiltse
{
    public class RingSegmentClickEventArgs : RoutedEventArgs
    {
        public RingItem Item { get; private set; }

        public RingSegmentClickEventArgs(RoutedEvent routedEvent, RingItem item)
            : base(routedEvent)
        {
            Item = item;
        }
    }
}