using BP.Kingdoms.Presentation;
using UnityEngine;

namespace BP.Kingdoms.Core
{
    [System.Serializable]
    public class GameServiceDependencies
    {
        [field:SerializeField] public CardBank CardBank { get; private set; }

    }
}
