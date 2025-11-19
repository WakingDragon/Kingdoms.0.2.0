using UnityEngine;
using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    public sealed class PlayerState
    {
        public PlayerId Id { get; private set; }
        public int Coins { get; private set; } = 0;
        public List<ICard> Hand { get; private set; }

        public PlayerState(PlayerId id, int coins, List<ICard> hand)
        {
            Id = id;
            Coins = coins;
            Hand = hand;
        }

        #region coins and hands
        public void SetCoins(int amount)
        {
            Coins = amount;
        }
        #endregion
    }
}

