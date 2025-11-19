using UnityEngine;
using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    /// <summary>
    /// All this does is setup a blank board with castles and sets the first player
    /// </summary>
    public static class DefaultGameStateFactory
    {
        public static GameState CreateInitial(int seed)
        {
            var state = new GameState(BoardSetup.BoardSize);

            //set rnd starting player
            var rng = new System.Random(seed);
            var first = rng.Next(0, 2) == 0 ? PlayerId.P1 : PlayerId.P2;            
            state.FirstTurn(first);

            //create players
            state.PlayerStates.Add(PlayerId.P1, CreatePlayer(PlayerId.P1));
            state.PlayerStates.Add(PlayerId.P2, CreatePlayer(PlayerId.P2));

            //set castles
            var p1 = BoardSetup.P1CastlePos;
            var p2 = BoardSetup.P2CastlePos;
            state.Board.Cells[p1.x, p1.y] = new CellState(isCastle: true, owner: TileOccupant.P1);
            state.Board.Cells[p2.x, p2.y] = new CellState(isCastle: true, owner: TileOccupant.P2);

            //create default hands/coins
            state.PlayerStates[PlayerId.P1].SetCoins(BoardSetup.DefaultStartingCoins);
            state.PlayerStates[PlayerId.P2].SetCoins(BoardSetup.DefaultStartingCoins);
            var startingCard = BoardSetup.DefaultStartingCards;
            state.PlayerStates[PlayerId.P1].Hand.Add(startingCard);
            state.PlayerStates[PlayerId.P2].Hand.Add(startingCard);

            return state;
        }

        private static PlayerState CreatePlayer(PlayerId id)
        {
            int coins = 0;
            List<ICard> hand = new List<ICard>();
            var playerState = new PlayerState(id, coins, hand);
            return playerState;
        }
    }
}
