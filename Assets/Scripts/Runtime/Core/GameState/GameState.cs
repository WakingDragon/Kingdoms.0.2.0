using UnityEngine;

namespace BP.Kingdoms.Core
{
    public sealed class GameState
    {
        public BoardState Board { get; private set; }
        public PlayerId CurrentPlayer { get; private set; } = PlayerId.P1;
        public TurnPhase turnPhase { get; private set; } = TurnPhase.Start;

        #region new game
        public GameState(int boardSize)
        {
            Board = new BoardState(boardSize);
        }

        public void FirstTurn(PlayerId firstPlayer)
        {
            CurrentPlayer = firstPlayer;
            turnPhase = TurnPhase.Start;
        }
        #endregion

        public void DebugState()
        {
            Debug.Log($"Current Player: {CurrentPlayer}, TurnPhase: {turnPhase}");
        }

        public void BeginTurn()
        {
            turnPhase = TurnPhase.Start;
        }

        public void AdvancePhase()
        {
            turnPhase = turnPhase switch
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
            turnPhase = TurnPhase.End;
            CurrentPlayer = (CurrentPlayer == PlayerId.P1) ? PlayerId.P2 : PlayerId.P1;
            turnPhase = TurnPhase.Start;
        }

    }
}
