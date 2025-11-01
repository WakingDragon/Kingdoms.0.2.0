using UnityEngine;

namespace BP.Kingdoms.Core
{
    /// <summary>
    /// All this does is setup a blank board with castles and sets the first player
    /// </summary>
    public static class GameInitializer
    {
        public static GameState CreateNewGame(int seed)
        {
            var state = new GameState(BoardSetup.BoardSize);

            //set rnd starting player
            var rng = new System.Random(seed);
            var first = rng.Next(0, 2) == 0 ? PlayerId.P1 : PlayerId.P2;            
            state.FirstTurn(first);

            //set castles
            var p1 = BoardSetup.P1CastlePos;
            var p2 = BoardSetup.P2CastlePos;
            state.Board.Cells[p1.x, p1.y] = new CellState(isCastle: true, owner: TileOccupant.P1);
            state.Board.Cells[p2.x, p2.y] = new CellState(isCastle: true, owner: TileOccupant.P2);

            return state;
        }
    }
}
