namespace BP.Kingdoms.Core
{
    public interface IRule
    {
        RuleResult Evaluate(in GameState state, in IGameAction action);
    }
}