using System;

namespace BP.Kingdoms.Core
{
    public sealed class GameService
    {
        public readonly IRule Rules;
        public GameState gameState { get; }

        public event Action<IGameAction, GameState> OnApplied; // for UI/network hooks

        public GameService(int seed)    //for new games
        {
            gameState = GameInitializer.CreateNewGame(seed);
        }

        public GameService(GameState initial, IRule rules) 
        { 
            gameState = initial; 
            Rules = rules; 
        }

        public RuleResult TryApply(IGameAction action)
        {
            var rr = Rules.Evaluate(gameState, action);
            if (rr.Verdict == RuleVerdict.Success)
            {
                Reducer.Apply(gameState, action);
                OnApplied?.Invoke(action, gameState);
            }
            return rr;
        }
    }
}