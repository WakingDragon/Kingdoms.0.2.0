using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using BP.Kingdoms.Core;

namespace BP.Kingdoms.Presentation
{
    public sealed class HandView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _debugText;

        public void UpdateFromPlayerState(PlayerState state, bool isLocalPlayer)
        {
            // Update the hand view based on the player state
            // This is a placeholder implementation
            DebugText(state, isLocalPlayer);
        }

        private void DebugText(PlayerState state, bool isLocalPlayer)
        {
            if (_debugText != null)
            {
                string cardsList = string.Join(", ", state.Hand);
                _debugText.text = $"{(isLocalPlayer ? "Local" : "Opponent")} Player:{state.Id}\nHand:\nCards: {state.Hand.Count} ({cardsList})\nCoins: {state.Coins}";
            }
            else
            {
                Debug.LogWarning("Debug TextMeshProUGUI is not assigned in HandView.");
            }
        }
    }
}
