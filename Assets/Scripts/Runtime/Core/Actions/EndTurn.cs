namespace BP.Kingdoms.Core
{
    public sealed class EndTurn : IGameAction 
    {
        public ActionKind Kind => ActionKind.Pass;
    }
}