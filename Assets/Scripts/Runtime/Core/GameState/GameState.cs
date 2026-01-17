using UnityEngine;
using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    public sealed class GameState
    {
        public BoardState Board { get; private set; }
        public PlayerId CurrentPlayerId { get; private set; } = PlayerId.P1;
        public Dictionary<PlayerId,PlayerState> PlayerStates { get; private set; } = new Dictionary<PlayerId, PlayerState>();
        public TurnPhase turnPhase { get; private set; } = TurnPhase.Start;

        #region new game
        public GameState(int boardSize)
        {
            Board = new BoardState(boardSize);
        }

        public void FirstTurn(PlayerId firstPlayer)
        {
            CurrentPlayerId = firstPlayer;
            turnPhase = TurnPhase.Start;
        }
        #endregion

        public void DebugState()
        {
            Debug.Log($"Current Player: {CurrentPlayerId}, TurnPhase: {turnPhase}");
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
            CurrentPlayerId = (CurrentPlayerId == PlayerId.P1) ? PlayerId.P2 : PlayerId.P1;
            turnPhase = TurnPhase.Start;
        }

    }
}
