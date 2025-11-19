using BP.Kingdoms.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BP.Kingdoms.Presentation
{
    /// <summary>
    /// Holds both the player hands (cards and coins)
    /// PlayerHand is an individual hand for a single player gets passed a reference to the HandView for updating
    /// </summary>
    public sealed class HandsView : MonoBehaviour
    {
        private PlayerId _localPlayerId = PlayerId.P1;
        public bool IsBuilt { get; private set; } = false;
        [SerializeField] private HandView localHand;
        [SerializeField] private HandView opponentHand;


        public void BuildHands(PlayerId localPlayerId, System.Action<int> onCardClicked, GameState state)
        {
            _localPlayerId = localPlayerId;

            UpdateFromGameState(state);

            IsBuilt = true;
        }

        public void UpdateFromGameState(GameState state)
        {
            //update the hand view based on the game state
            (PlayerState localState, PlayerState opponentState) = GetPlayerStates(state);
            localHand.UpdateFromPlayerState(localState, isLocalPlayer: true);
            opponentHand.UpdateFromPlayerState(opponentState, isLocalPlayer: false);
        }

        private (PlayerState localState, PlayerState opponentState) GetPlayerStates(GameState state)
        {
            PlayerState localState = state.PlayerStates[_localPlayerId];
            PlayerId opponentId = (_localPlayerId == PlayerId.P1) ? PlayerId.P2 : PlayerId.P1;
            PlayerState opponentState = state.PlayerStates[opponentId];
            return (localState, opponentState);
        }
    }
}
