using BP.Kingdoms.Core;

namespace BP.Kingdoms.Rules
{
    public sealed class MustBeOnBoard : IRule
    {
        public RuleResult Evaluate(GameState state, IGameAction action)
        {
            if (action.Kind == ActionKind.PlacePiece)
            {
                var placeAction = (PlaceMarker)action;
                if (!placeAction.At.InBounds(state.Board.Size)) return RuleResult.Fail("Out of bounds");
            }
            return RuleResult.Success("Cell in bounds.");
        }
    }
}