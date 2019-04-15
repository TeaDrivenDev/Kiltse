namespace TeaDriven.Kiltse
{
    public class RingItem
    {
        public int ItemIndex { get; }

        public object Item { get; }

        public string Name => this.Item.ToString();

        public RingItem(int itemIndex, object item)
        {
            ItemIndex = itemIndex;
            Item = item;
        }
    }
}