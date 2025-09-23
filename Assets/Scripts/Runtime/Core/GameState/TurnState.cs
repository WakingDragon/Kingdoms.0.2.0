namespace BP.Kingdoms.Core
{
    public sealed class TurnState
    {
        public PlayerId Active { get; private set; } = PlayerId.P1;
        public int TurnNumber { get; private set; } = 1;
        public string Phase { get; private set; } = "Main";

        public void NextTurn()
        {
            Active = Active == PlayerId.P1 ? PlayerId.P2 : PlayerId.P1;
            TurnNumber++;
        }
    }
}
