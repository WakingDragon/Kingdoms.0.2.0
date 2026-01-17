using UnityEngine;

namespace BP.Kingdoms.Core
{
    public sealed class BoardState
    {
        public readonly int BoardSize;
        public CellState[,] Cells { get; }

        public BoardState(int boardSize)
        {
            BoardSize = boardSize;
            Cells = new CellState[BoardSize, BoardSize];
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    Cells[x, y] = new CellState();
                }
            }
        }

        public void DebugBoard()
        {
            Debug.Log("Board State:");
            for (int y = 0; y < BoardSize; y++)
            {
                string row = "";
                for (int x = 0; x < BoardSize; x++)
                {
                    row += "[" + x + "," + y;
                    if (Cells[x, y].IsCastle)
                    {
                        row += Cells[x, y].Owner == TileOccupant.P1 ? " Castle P1" : " Castle P2";
                    }
                    else if (Cells[x, y].Occupied)
                    {
                        row += Cells[x, y].Owner == TileOccupant.P1 ? " X1" : " X2";
                    }
                    else
                    {
                        //row += " Empty";
                    }
                    row += "] ";
                }
                Debug.Log(row);
            }
        }

        #region board helpers
        public bool InBounds(int x, int y) => (x >= 0 && y >= 0 && x < BoardSize && y < BoardSize);

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

