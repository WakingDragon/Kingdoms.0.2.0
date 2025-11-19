using UnityEngine;
using TMPro;

namespace BP.Kingdoms.Presentation
{
    public sealed class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        public void SetCard(CardData cardData)
        {
            // Placeholder implementation for setting the card's visual representation
            _titleText.text = cardData.cardName;
        }
    }
}
