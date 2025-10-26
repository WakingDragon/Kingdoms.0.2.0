namespace BP.Kingdoms.Core
{
    public sealed class BoardState
    {
        public readonly int Size;
        public CellState[,] Cells { get; }

        public BoardState(int boardSize)
        {
            Size = boardSize;
            Cells = new CellState[Size, Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Cells[x, y] = new CellState();
                }
            }
        }

        #region board helpers
        public bool InBounds(int x, int y) => (x >= 0 && y >= 0 && x < Size && y < Size);

        public TileOccupant GetOccupant(int x, int y)
        {
            if (Cells[x, y].Occupied) return Cells[x, y].Owner;
            return TileOccupant.None;
        }

        public (bool isCastle, TileOccupant id) IsCastle(int x, int y)
        {
            if (Cells[x, y].IsCastle) return (true, Cells[x, y].Owner);
            return (false, TileOccupant.None);
        }
        #endregion
    }
}

