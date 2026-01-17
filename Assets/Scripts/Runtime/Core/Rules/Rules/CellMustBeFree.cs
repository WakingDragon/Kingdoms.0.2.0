using BP.Kingdoms.Core;

namespace BP.Kingdoms.Rules
{
    public sealed class CellMustBeFree : IRule
    {
        public RuleResult Evaluate(GameState state, IGameAction action)
        {
            if (action.Kind == ActionKind.PlacePiece)
            {
                var placeAction = (PlaceMarker)action;
                var cell = state.Board.Cells[placeAction.At.X, placeAction.At.Y];
                if (cell.Occupied) return RuleResult.Fail("Cell occupied");
            }
            return RuleResult.Success("Cell is free");
        }
    }
}