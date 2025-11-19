using UnityEngine;
using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    public sealed class PlayerState
    {
        public PlayerId Id { get; private set; }
        public int Coins { get; private set; } = 0;
        public List<ICard> Cards { get; private set; }

        public PlayerState(PlayerId id, int coins, List<ICard> cards)
        {
            Id = id;
            Coins = coins;
            Cards = cards;
        }

        #region coins and hands
        public void SetCoins(int amount)
        {
            Coins = amount;
        }
        #endregion
    }
}

