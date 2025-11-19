using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using BP.Kingdoms.Core;
using System.Collections.Generic;
using System;

namespace BP.Kingdoms.Presentation
{
    public sealed class HandView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _debugText;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private CardBank _cardBank;
        [SerializeField] private RectTransform _gridRoot;   // GridLayoutGroup holder
        private List<CardView> _cardsInHand = new();

        public void UpdateFromPlayerState(PlayerState state, Action<int> onCardClicked, bool isLocalPlayer)
        {
            // Update the hand view based on the player state
            // This is a placeholder implementation
            DebugText(state, isLocalPlayer);
            UpdateCards(state.Cards, onCardClicked);
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

        private void UpdateCards(List<ICard> cards, Action<int> onCardClicked)
        {
            //clear cards
            _cardsInHand.Clear();
            foreach (Transform child in _gridRoot) Destroy(child.gameObject);

            // Instantiate new card views based on the cards in hand
            foreach (var card in cards)
            {
                //get the data obj 
                var cardData = _cardBank.GetCardDisplayDataByKey(card.CardKey);
                //Debug.Log(cardData);
                var cardView = Instantiate(_cardViewPrefab, _gridRoot);
                cardView.SetCard(cardData, onCardClicked);
                _cardsInHand.Add(cardView);
            }
        }
    }
}
