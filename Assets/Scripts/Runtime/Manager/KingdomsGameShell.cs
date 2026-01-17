using BP.Kingdoms.Core;
using BP.Kingdoms.Presentation;
using UnityEngine;

namespace BP.Kingdoms.Manager
{
    /// <summary>
    /// Pure data (not monobehaviour) instance of the master game controller for managing startup
    /// </summary>
    public class KingdomsGameShell : IGameShell
    {
        private GameServiceDependencies _gameServiceDependencies;
        private UIGameObjectDependencies _uiGameObjectDependencies;

        public KingdomsGameShell(GameServiceDependencies gameServiceDependencies, UIGameObjectDependencies uiDependencies)
        {
            _gameServiceDependencies = gameServiceDependencies;
            _uiGameObjectDependencies = uiDependencies;
        }
    }
}
