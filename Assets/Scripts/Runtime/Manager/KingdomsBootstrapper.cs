using BP.Kingdoms.Core;
using BP.Kingdoms.Presentation;
using System.Collections.Generic;
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
        [SerializeField] private GameServiceDependencies gameServiceDependencies;
        private GameService _gameService;
        [SerializeField] private BoardPresenter _boardPresenter;


        [SerializeField] private List<CardData> _startingCards;
        [SerializeField] private int _startingCoins = 0;

        private void Start()
        {
            if (_bootstrap) Bootstrap();
        }

        private void Bootstrap()
        {
            InjectDebugStartingHand();
            _gameService = new GameService(_seed, gameServiceDependencies);  //for new game, pass in key stuff
            _boardPresenter.Init(_gameService, _thisPlayer);

            _gameService.gameState.DebugState();
            _gameService.PushHints();
        }

        private void InjectDebugStartingHand()
        {
            //coins
            if( _startingCoins > 0)
            {
                BoardSetup.SetDefaultStartingCoins(_startingCoins);
            }

            //cards
            if (_startingCards.Count == 0) return;

            List<ICard> cards = new();
            foreach (var card in _startingCards)
            {
                cards.Add(card.GetCard());
            }
            BoardSetup.InjectStartingCards(cards);
        }
    }
}
