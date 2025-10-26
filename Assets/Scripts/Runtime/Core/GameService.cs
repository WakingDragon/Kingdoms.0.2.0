using System;

namespace BP.Kingdoms.Core
{
    public sealed class GameService
    {
        private readonly IRule _rules;
        public GameState State { get; }


        public event Action<IGameAction, GameState> OnApplied; // for UI/network hooks

        public GameService(GameState initial, IRule rules) { State = initial; _rules = rules; }

        public RuleResult TryApply(IGameAction action)
        {
            var rr = _rules.Evaluate(State, action);
            if (rr.Verdict == RuleVerdict.Success)
            {
                Reducer.Apply(State, action);
                OnApplied?.Invoke(action, State);
            }
            return rr;
        }
    }
}