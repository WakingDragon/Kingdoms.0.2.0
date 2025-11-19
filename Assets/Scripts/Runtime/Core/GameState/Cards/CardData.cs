using UnityEngine;
using BP.Kingdoms.Core;

namespace BP.Kingdoms.Presentation
{
    [CreateAssetMenu(fileName = "new_cardData", menuName = "Kingdoms/Card")]
    public class CardData : ScriptableObject
    {
        [Header("Card Key")]
        [field:SerializeField] public CardKey cardKey { get; private set; }

        [Header("Card Display")]
        [field:SerializeField] public string cardName { get; private set; }

        public ICard GetCard()
        {
            return CardFactory.GetICardFromKey(cardKey);
        }

    }
}

