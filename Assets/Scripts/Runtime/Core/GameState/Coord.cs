namespace BP.Kingdoms.Core
{    public readonly struct Coord
    {
        public int X { get; }
        public int Y { get; }
        public Coord(int x, int y) { X = x; Y = y; }
        public bool InBounds(int size) => X >= 0 && Y >= 0 && X < size && Y < size;
        public override string ToString() => $"({X},{Y})";
    }
}

