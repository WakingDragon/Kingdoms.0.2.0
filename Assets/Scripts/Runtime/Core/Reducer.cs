namespace BP.Kingdoms.Core
{
    public static class Reducer
    {
        public static void Apply(GameState s, IGameAction a)
        {
            switch (a)
            {
                case PlaceMarker pm:
                    var cell = s.Board.Cells[pm.At.X, pm.At.Y];
                    cell.Occupy(pm.By);
                    break;
                case EndTurn _:
                    s.Turn.NextTurn();
                    break;
            }
        }
    }
}