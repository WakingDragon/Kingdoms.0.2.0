namespace BP.Kingdoms.Core
{
    public sealed class GameState
    {
        public BoardState Board { get; } = new BoardState(9);
        public PlayerId CurrentPlayer { get; private set; } = PlayerId.P1;
        public TurnPhase Phase { get; private set; } = TurnPhase.Start;

        private int _seed = 0;

        public int Seed
        {
            get => _seed;
            set => _seed = value;
        }

        public void FirstTurn(PlayerId firstPlayer)
        {
            CurrentPlayer = firstPlayer;
            Phase = TurnPhase.Start;
        }

        public void BeginTurn()
        {
            Phase = TurnPhase.Start;
        }

        public void AdvancePhase()
        {
            Phase = Phase switch
            {
                TurnPhase.Start => TurnPhase.CardEffect,
                TurnPhase.CardEffect => TurnPhase.Placement,
                TurnPhase.Placement => TurnPhase.Flip,
                TurnPhase.Flip => TurnPhase.End,
                _ => TurnPhase.End
            };
        }

        public void EndTurn()
        {
            Phase = TurnPhase.End;
            CurrentPlayer = (CurrentPlayer == PlayerId.P1) ? PlayerId.P2 : PlayerId.P1;
            Phase = TurnPhase.Start;
        }

    }
}
