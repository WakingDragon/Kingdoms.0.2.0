using BP.Kingdoms.Core;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Kingdoms.Presentation
{

    public class BoardPresenter : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private BoardThemeAsset theme;

        // Inject or assign in inspector for a local hot-seat demo
        public CompositeRuleSet Rules { get; set; }
        public GameState Game { get; set; }

        private HashSet<Vector2Int> _currentLegals = new();

        private const int BoardSize = 9;

        private void Start()
        {
            // Build visuals once
            boardView.BuildGrid(BoardSize, OnCellClicked);

            // Paint checkerboard and castles
            PaintStaticBoard();

            // First render
            RefreshAll();
        }

        private void PaintStaticBoard()
        {
            for (int y = 0; y < BoardSize; y++)
                for (int x = 0; x < BoardSize; x++)
                {
                    var cell = boardView.GetCell(new Vector2Int(x, y));
                    bool dark = ((x + y) & 1) == 1;
                    cell.SetBackground(dark ? theme.cellDark : theme.cellLight);
                    TrySetCastle(x, y, cell);
                }
        }

        private void RefreshAll()
        {
            // Pieces
            for (int y = 0; y < BoardSize; y++)
                for (int x = 0; x < BoardSize; x++)
                {
                    var occ = Game.Board.GetOccupant(x, y);
                    var cellView = boardView.GetCell(new Vector2Int(x, y));

                    if (occ == TileOccupant.None)
                    {
                        cellView.SetPiece(null, Color.white, false);
                    }
                    else
                    {
                        cellView.SetPiece(theme.pieceDisc, GetPlayerColour(occ), true);
                    }
                }

            // Legal moves for current player
            boardView.ClearAllHighlights();
            _currentLegals = new HashSet<Vector2Int>(Rules.GetLegalPlacements(Game)); // returns IEnumerable<Coord>
            foreach (var c in _currentLegals)
            {
                boardView.GetCell(new Vector2Int(c.x, c.y)).SetHighlight(true);
            }
        }

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

        //private void ApplyResolvedTurnToView(ResolvedTurn result)
        //{
        //    // Keep it simple for greybox: fast step-wise updates.
        //    foreach (var step in result.Steps)
        //    {
        //        switch (step.Type)
        //        {
        //            case ResolutionStepType.CardEffect:
        //                // (optional) flash HUD banner
        //                break;
        //            case ResolutionStepType.Placement:
        //                var cv = boardView.GetCell(new Vector2Int(step.X, step.Y));
        //                bool isA = result.ActivePlayer == PlayerId.A;
        //                cv.SetPiece(theme.pieceDisc, isA ? theme.p1Colour : theme.p2Colour, true);
        //                break;
        //            case ResolutionStepType.Flip:
        //                var fv = boardView.GetCell(new Vector2Int(step.X, step.Y));
        //                bool toA = step.FlipTo == PlayerId.A;
        //                fv.SetPiece(theme.pieceDisc, toA ? theme.p1Colour : theme.p2Colour, true);
        //                break;
        //        }
        //    }
        //}

        private void TrySetCastle(int x, int y, CellView cell)
        {
            (bool isCastle, TileOccupant id) = Game.Board.IsCastle(x, y);
            if (isCastle)
            {
                if(id == TileOccupant.P1)
                cell.SetCastle(true, theme.castleSprite, GetPlayerColour(id));
            }
        }

        private Color GetPlayerColour(TileOccupant id)
        {
            return (id == TileOccupant.P1 ? theme.p1Colour : theme.p2Colour);
        }
    }
}