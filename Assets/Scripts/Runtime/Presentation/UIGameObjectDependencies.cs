using UnityEngine;

namespace BP.Kingdoms.Presentation
{
    [System.Serializable]
    public class UIGameObjectDependencies
    {
        [field: SerializeField] public BoardPresenter _boardPresenter { get; private set; }

    }
}
