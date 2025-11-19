using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using BP.Kingdoms.Core;
using System.Collections.Generic;

namespace BP.Kingdoms.Presentation
{
    public sealed class HandView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _debugText;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private CardBank _cardBank;
        private List<CardView> _cardsInHand;

        public void UpdateFromPlayerState(PlayerState state, bool isLocalPlayer)
        {
            // Update the hand view based on the player state
            // This is a placeholder implementation
            DebugText(state, isLocalPlayer);
            UpdateCards(state.Cards);
        }

        private void DebugText(PlayerState state, bool isLocalPlayer)
        {
            if (_debugText != null)
            {
                string cardsList = string.Join(", ", state.Cards);
                _debugText.text = $"{(isLocalPlayer ? "Local" : "Opponent")} Player:{state.Id}\nHand:\nCards: {state.Cards.Count} ({cardsList})\nCoins: {state.Coins}";
            }
            else
            {
                Debug.LogWarning("Debug TextMeshProUGUI is not assigned in HandView.");
            }
        }

        private void UpdateCards(List<ICard> cards)
        {
            // Clear existing card views
            if(_cardsInHand == null)
            {
                _cardsInHand = new List<CardView>();
            }
            foreach (var cardView in _cardsInHand)
            {
                Destroy(cardView.gameObject);
            }
            _cardsInHand.Clear();

            // Instantiate new card views based on the cards in hand
            foreach (var card in cards)
            {
                //get the data obj 
                var cardData = _cardBank.GetCardDisplayDataByKey(card.CardKey);
                Debug.Log(cardData);
                var cardView = Instantiate(_cardViewPrefab, transform);
                cardView.SetCard(cardData);
                _cardsInHand.Add(cardView);
            }
        }
    }
}
