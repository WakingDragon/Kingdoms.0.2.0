namespace BP.Kingdoms.Core
{
    public sealed class GameState
    {
        public BoardState Board { get; } = new BoardState();
        public TurnState Turn { get; } = new TurnState();
    }
}
