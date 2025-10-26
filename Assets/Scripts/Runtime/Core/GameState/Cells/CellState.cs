namespace BP.Kingdoms.Core
{
    public sealed class CellState
    {
        public TileOccupant Owner { get; private set; } = TileOccupant.None;
        public bool Occupied { get; private set; }
        public bool IsCastle { get; }


        public CellState(bool isCastle = false) { IsCastle = isCastle; }


        // Narrow, intentional mutators keep state safe
        public void Occupy(PlayerId by) 
        { 
            Occupied = true; 
            Owner = (by == PlayerId.P1 ? TileOccupant.P1 : TileOccupant.P2); 
        }

        public void Vacate() { Occupied = false; Owner = TileOccupant.None; }
    }
}

