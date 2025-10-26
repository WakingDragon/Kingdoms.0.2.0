using UnityEngine;

namespace BP.Kingdoms.Presentation
{
    [CreateAssetMenu(fileName = "new_boardTheme", menuName = "Kingdoms/Board Theme")]
    public class BoardThemeAsset : ScriptableObject
    {
        [Header("Pieces")]
        public Sprite pieceDisc;
        public Color p1Colour = Color.white;
        public Color p2Colour = Color.black;

        [Header("Cell")]
        public Color cellLight = new Color(0.85f, 0.85f, 0.85f);
        public Color cellDark = new Color(0.75f, 0.75f, 0.75f);

        [Header("Highlight")]
        public Color highlightColor = new Color(1f, 1f, 0f, 0.35f);

        [Header("Castles")]
        public Sprite castleSprite;
        public Color castleColor = Color.cyan;
    }
}

