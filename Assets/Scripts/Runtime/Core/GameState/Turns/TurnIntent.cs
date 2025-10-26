namespace BP.Kingdoms.Core
{
    public sealed class TurnIntent
    {
        public int X { get; }
        public int Y { get; }
        private TurnIntent(int x, int y) { X = x; Y = y; }
        public static TurnIntent PlaceAt(int x, int y) => new TurnIntent(x, y);
    }
}
