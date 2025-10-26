using BP.Kingdoms.Core;

namespace BP.Kingdoms.Rules
{
    public sealed class MustBeActivePlayer : IRule
    {
        public RuleResult Evaluate(GameState state, IGameAction action)
        {
            if (action.Kind == ActionKind.PlacePiece)
            {
                var placeAction = (PlaceMarker)action;
                if (placeAction.By != state.CurrentPlayer) return RuleResult.Fail("Not your turn");
            }
            return RuleResult.Success("Is active player");
        }
    }
}