namespace BP.Kingdoms.Core
{
    public interface IRule
    {
        public RuleResult Evaluate(GameState state, IGameAction action);
    }
}