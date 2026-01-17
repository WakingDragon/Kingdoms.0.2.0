using UnityEngine;
using BP.Kingdoms.Core;
using System.Collections.Generic;

namespace BP.Kingdoms.Presentation
{
    [CreateAssetMenu(fileName = "CardBank", menuName = "Kingdoms/(single) Card Bank")]
    public class CardBank : ScriptableObject
    {
        [Header("Cards In Bank")]
        [field:SerializeField] public CardData[] cardsInBank { get; private set; }

        //TODO some kind of compiler check to make sure all cards and card data match up?

        public CardData GetCardDisplayDataByKey(CardKey key)
        {
            foreach (var cardData in cardsInBank)
            {
                if (cardData.cardKey == key)
                {
                    return cardData;
                }
            }
            Debug.LogError($"Card with key {key} not found in CardBank.");
            return null;
        }
    }
}

