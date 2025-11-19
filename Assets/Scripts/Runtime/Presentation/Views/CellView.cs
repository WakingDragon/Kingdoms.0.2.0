using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace BP.Kingdoms.Presentation
{
    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        //[SerializeField] private BoardThemeAsset _theme;
        [SerializeField] private Image _background;
        [SerializeField] private Image _piece;
        [SerializeField] private Image _highlight;
        [SerializeField] private Image _castleMark;
        [SerializeField] private TextMeshProUGUI _debugText;
        [SerializeField] private Color _hintColor;

        public Vector2Int Coords { get; private set; }
        public UnityEvent<CellView> OnClicked = new();

        public void Init(Vector2Int coords, UnityAction<CellView> onClick)
        {
            Coords = coords;
            OnClicked.RemoveAllListeners();
            OnClicked.AddListener(onClick);
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => OnClicked.Invoke(this));
            DebugText($"{coords.x},{coords.y}");

            //init sprites etc.
            _castleMark.enabled = false;
            _highlight.enabled = false;
        }

        public void SetPiece(Sprite spriteOrNull, Color color, bool visible)
        {
            _piece.enabled = visible;
            if (visible)
            {
                _piece.sprite = spriteOrNull;
                _piece.color = color;
            }
        }

        public void SetCastle(bool isCastle, Sprite castleSprite, Color color)
        {
            _castleMark.enabled = isCastle;
            if (isCastle)
            {
                _castleMark.sprite = castleSprite;
                _castleMark.color = color;
            }

            DebugText($"{Coords.x},{Coords.y}\nCastle: {isCastle}");
        }

        public void SetHighlight(bool on) => _highlight.enabled = on;

        public void SetHint(bool on) 
        {
            SetHighlight(on);
            if (on) _highlight.color = _hintColor;
        }

        private void DebugText(string text)
        {
            _debugText.text = text;
        }
    }
}
