using BP.Kingdoms.Core;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Kingdoms.Presentation
{

    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private HandsView handsView;
        [SerializeField] private BoardThemeAsset theme;
        private PlayerId _thisPlayer = PlayerId.P1;

        // Inject, or assign in inspector for a local hot-seat demo
        public CompositeRuleSet Rules { get; set; }
        private GameService _service;

        private HashSet<Vector2Int> _currentLegals = new();

        private int _boardSize = 9;

        #region init
        public void Init(GameService service, PlayerId thisPlayer)
        {
            _boardSize = BoardSetup.BoardSize;
            _service = service;
            _thisPlayer = thisPlayer;

            //sign up for changes
            _service.OnHints += OnHints;
            _service.OnApplied += OnChangeApplied;

            // Build visuals once
            if(!boardView.IsBuilt) boardView.BuildGrid(_boardSize, OnCellClicked, _thisPlayer);

            if(!handsView.IsBuilt) handsView.BuildHands(_thisPlayer, OnCardClicked, _service.gameState);

            // NB: this should not be required as 'refresh from game state' should provide all the info needed
            //TODO refactor refreshall to UpdateFromGameState [gamestate has been init'd so should be just castles and whatnot]
            //InitialiseBoardPresentation(_service.gameState);
            ApplyNewGameState(_service.gameState);

        }
        #endregion

        #region hints
        private void OnHints(List<Coord> validCoords)
        {
            UpdateHints(validCoords);
        }

        private void UpdateHints(List<Coord> validCoords)
        {
            foreach (var c in validCoords)
            {
                var cell = boardView.GetCell(new Vector2Int(c.X, c.Y));
                if (cell != null)
                {
                    //Debug.Log($"Highlighting cell at {c.X},{c.Y}");
                    cell.SetHint(true);
                }
            }
        }
        #endregion

        private void OnChangeApplied(IGameAction action, GameState state)
        {
            //TODO apply diffs and animations for turn steps in sequence (IGameAction or ResolvedTurn??)
            ApplyNewGameState(state);
        }

        #region final board paint
        private void ApplyNewGameState(GameState state)
        {
            //TODO update cards
            //TODO update coins
            //TODO update whose turn?
            UpdateBoardView(state);
            handsView.UpdateFromGameState(state); //should just be receiving dumb hand data
        }

        private void UpdateBoardView(GameState state)
        {
            for (int y = 0; y < _boardSize; y++)
            {
                for (int x = 0; x < _boardSize; x++)
                {
                    var cellView = boardView.GetCell(new Vector2Int(x, y));
                    if (IsCastleSetCastle(x, y, cellView, state)) //sets castle if castle
                    {
                        continue;
                    }

                    var occ = state.Board.GetOccupant(x, y);                   

                    if (occ == TileOccupant.None)
                    {
                        cellView.SetPiece(null, Color.white, false);
                    }
                    else
                    {
                        cellView.SetPiece(theme.pieceDisc, GetPlayerColour(occ), true);
                    }
                }
            }
        }

        private bool IsCastleSetCastle(int x, int y, CellView cell, GameState state)
        {
            (bool isCastle, TileOccupant id) = state.Board.IsCastle(x, y);
            if (isCastle)
            {
                if (id == TileOccupant.P1 || id == TileOccupant.P2)
                {
                    cell.SetCastle(true, theme.castleSprite, GetPlayerColour(id));
                }
            }
            return isCastle;
        }

        private Color GetPlayerColour(TileOccupant id)
        {
            return (id == TileOccupant.P1 ? theme.p1Colour : theme.p2Colour);
        }
        #endregion

        
        private void OnCellClicked(CellView cell)
        {
            var c = cell.Coords;
            if (!_currentLegals.Contains(c)) return;

            // Build a TurnIntent: card (if any) first, then placement
            var intent = TurnIntent.PlaceAt(c.x, c.y); // plus: attach selected card if UI chose one

            //var result = Rules.ResolveTurn(Game, intent);   // returns ResolvedTurn + events
            //ApplyResolvedTurnToView(result);                // short anims: card-> placement-> flips
            //RefreshAll();
        }

        private void OnCardClicked(int cardId)
        {
            //change card view
            //tell teh engine to return new ruleset
            //tell the board present to update hints based on new ruleset
        }

        private void OnDestroy()
        {
            if (_service != null) _service.OnApplied -= OnChangeApplied;
        }
    }
}