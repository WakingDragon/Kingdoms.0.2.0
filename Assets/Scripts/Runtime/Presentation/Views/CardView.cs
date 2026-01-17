using UnityEngine;
using TMPro;
using System;

namespace BP.Kingdoms.Presentation
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        private CardMouseInteractor _mouseInteractor;

        private void OnEnable()
        {
            _mouseInteractor = GetComponent<CardMouseInteractor>();
        }

        public void SetCard(CardData cardData, Action<int> onCardClicked)
        {
            // Placeholder implementation for setting the card's visual representation
            _titleText.text = cardData.cardName;
            _mouseInteractor.SetClickAction(onCardClicked, (int)cardData.cardKey);
        }
    }
}
