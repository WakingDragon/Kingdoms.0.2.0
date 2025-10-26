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

        private readonly Dictionary<Vector2Int, CellView> _cells = new();

        public void BuildGrid(int size, System.Action<CellView> clickHandler)
        {
            _cells.Clear();
            foreach (Transform child in gridRoot) Destroy(child.gameObject);

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    var cv = Instantiate(cellPrefab, gridRoot);
                    var coords = new Vector2Int(x, y);
                    cv.Init(coords, _ => clickHandler(cv));
                    _cells[coords] = cv;
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
