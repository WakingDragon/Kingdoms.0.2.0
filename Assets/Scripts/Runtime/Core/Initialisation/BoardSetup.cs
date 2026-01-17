using UnityEngine;
using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    public static class BoardSetup
    {
        private const int _boardSize = 9;

        //positions are zero-indexed so 3,3 is the 4th cell from left and 4th from bottom
        private const int _p1CastlePosX = 3;    
        private const int _p1CastlePosY = 3;
        private const int _p2CastlePosX = 5;
        private const int _p2CastlePosY = 5;

        public static int BoardSize => _boardSize;
        public static Vector2Int P1CastlePos => new Vector2Int(_p1CastlePosX, _p1CastlePosY);
        public static Vector2Int P2CastlePos => new Vector2Int(_p2CastlePosX, _p2CastlePosY);

        //hands
        private static int _startingCoins = 0;
        public static int DefaultStartingCoins => _startingCoins;
        public static void SetDefaultStartingCoins(int coins) //for testing purposes
        {
            _startingCoins = coins;
        }
        private static List<ICard> _startingCards = new List<ICard> { new NullCard() };
        public static List<ICard> GetStartingCards => _startingCards;
        public static void InjectStartingCards(List<ICard> cards) //for testing purposes
        {
            _startingCards.AddRange(cards);
        }

    }
}
