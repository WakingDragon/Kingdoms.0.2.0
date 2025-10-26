using System.Collections.Generic;
using UnityEngine;

namespace BP.Kingdoms.Core
{
    // Rules container used by the presenter
    public sealed class CompositeRuleSet : IRule
    {
        private readonly int _boardSize;
        private readonly List<IRule> _rules;

        public CompositeRuleSet(int boardSize, params IRule[] rules)
        {
            _boardSize = boardSize;
            _rules = new List<IRule>(rules); 
        }

        // --- 1) What cells can the current player place on?
        // For now: any empty tile (you’ll replace with real legality logic)
        public IEnumerable<Vector2Int> GetLegalPlacements(GameState game)
        {
            var b = game.Board;
            for (int y = 0; y < _boardSize; y++)
            {
                for (int x = 0; x < _boardSize; x++)
                {
                    if (b.GetOccupant(x, y) == TileOccupant.None)
                        yield return new Vector2Int(x, y);
                }
            }
        }

        public RuleResult Evaluate(GameState state, IGameAction action)
        {
            foreach (var rule in _rules)
            {
                var ruleResult = rule.Evaluate(state, action);
                if (ruleResult.Verdict == RuleVerdict.Fail) return ruleResult; // short-circuit
            }
            return RuleResult.Success("Unassigned rule result?");
        }
    }
}