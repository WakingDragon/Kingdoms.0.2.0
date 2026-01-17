using BP.Kingdoms.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BP.Kingdoms.Presentation
{
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private RectTransform gridRoot;   // GridLayoutGroup holder
        [SerializeField] private CellView cellPrefab;
        private PlayerId _thisPlayer = PlayerId.P1;

        private readonly Dictionary<Vector2Int, CellView> _cells = new();

        public bool IsBuilt => _cells.Count > 0;

        public void BuildGrid(int size, System.Action<CellView> clickHandler, PlayerId thisPlayer)
        {
            _cells.Clear();
            foreach (Transform child in gridRoot) Destroy(child.gameObject);

            _thisPlayer = thisPlayer;
            SetupGridView(thisPlayer);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var cellView = Instantiate(cellPrefab, gridRoot);
                    var coords = new Vector2Int(x, y);
                    cellView.gameObject.name = $"Cell_{x}_{y}";
                    cellView.Init(coords, _ => clickHandler(cellView));
                    _cells[coords] = cellView;
                }
            }
        }

        private void SetupGridView(PlayerId forPlayer)
        {
            var gridLayoutGroup = gridRoot.GetComponent<GridLayoutGroup>();

            if (forPlayer == PlayerId.P2)
            {
                gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperRight;
            }
            else
            {
                gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft;
            }
        }


        public CellView GetCell(Vector2Int c) => _cells[c];

        public void Highlight(IEnumerable<Vector2Int> coords, bool on)
        {
            foreach (var c in coords) _cells[c].SetHighlight(on);
        }

        public void ClearAllHighlights()
        {
            foreach (var kv in _cells) kv.Value.SetHighlight(false);
        }
    }
}
