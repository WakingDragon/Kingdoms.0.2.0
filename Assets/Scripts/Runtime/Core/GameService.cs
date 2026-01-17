using System.Collections.Generic;
using System;
using UnityEngine;

namespace BP.Kingdoms.Core
{
    public sealed class GameService
    {
        public readonly IRule Rules;
        public GameState gameState { get; }
        public GameServiceDependencies Dependencies { get; private set; }

        public event Action<List<Coord>> OnHints; // for UI/network hooks
        public event Action<IGameAction, GameState> OnApplied; // for UI/network hooks

        public GameService(int seed, GameServiceDependencies dependencies)    //for new games
        {
            Dependencies = dependencies;
            gameState = DefaultGameStateFactory.CreateInitial(seed);
        }

        public GameService(GameState initial, IRule rules) 
        { 
            gameState = initial; 
            Rules = rules; 
        }

        public void PushHints()
        {
            var hints = new List<Coord>();
            hints.Add(new Coord(1, 1)); //placeholder
            hints.Add(new Coord(2, 1)); //placeholder
            OnHints?.Invoke(hints);
            //Debug.Log($"Trying to apply hints.");
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