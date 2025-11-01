using BP.Kingdoms.Core;
using BP.Kingdoms.Presentation;
using UnityEngine;

namespace BP.Kingdoms.Manager
{
    /// <summary>
    /// Temporary bootstrapper to initialize the game state and services for a local hot-seat demo.
    /// Does not account for networking, saving/loading, or other advanced features.
    /// </summary>
    public class KingdomsBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _bootstrap = true;
        [SerializeField] private int _seed = 1;
        [SerializeField] private PlayerId _thisPlayer = PlayerId.P1;
        private GameService _gameService;
        [SerializeField] private BoardPresenter _boardPresenter;

        private void Start()
        {
            if (_bootstrap) Bootstrap();
        }

        private void Bootstrap()
        {
            _gameService = new GameService(_seed);  //for new game, just pass in seed
            _boardPresenter.Init(_gameService, _thisPlayer);

            _gameService.gameState.DebugState();
        }
    }
}
