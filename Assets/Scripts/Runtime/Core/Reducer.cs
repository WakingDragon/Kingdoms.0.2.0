namespace BP.Kingdoms.Core
{
    public static class Reducer
    {
        public static void Apply(GameState state, IGameAction action)
        {
            switch (action)
            {
                case PlaceMarker placeAction:
                    var cell = state.Board.Cells[placeAction.At.X, placeAction.At.Y];
                    cell.Occupy(placeAction.By);
                    break;
                case EndTurn _:
                    state.EndTurn();
                    break;
            }
        }
    }
}