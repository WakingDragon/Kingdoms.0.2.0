namespace BP.Kingdoms.Core
{
    public readonly struct Coord
    {
        public int X { get; }
        public int Y { get; }
        public Coord(int x, int y) { X = x; Y = y; }
        public bool InBounds(int size) => X >= 0 && Y >= 0 && X < size && Y < size;
    }

    public enum PlayerId { None = 0, P1 = 1, P2 = 2 }

    public sealed class CellState
    {
        public PlayerId Owner { get; private set; } = PlayerId.None;
        public bool Occupied { get; private set; }
        public bool IsCastle { get; }


        public CellState(bool isCastle = false) { IsCastle = isCastle; }


        // Narrow, intentional mutators keep state safe
        public void Occupy(PlayerId by) { Occupied = true; Owner = by; }
        public void Vacate() { Occupied = false; Owner = PlayerId.None; }
    }

    public sealed class BoardState
    {
        public const int Size = 9; // Phase 1 fixed
        public CellState[,] Cells { get; } = new CellState[Size, Size];

        public BoardState()
        {
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    Cells[x, y] = new CellState();
        }
    }
}

