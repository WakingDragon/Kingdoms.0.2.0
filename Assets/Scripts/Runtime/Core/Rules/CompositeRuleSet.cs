using System.Collections.Generic;
using UnityEngine;

namespace BP.Kingdoms.Core
{
    // Rules container used by the presenter
    public sealed class CompositeRuleSet : IRule
    {
        private readonly List<IRule> _rules;

        public CompositeRuleSet(params IRule[] rules)
        {
            _rules = new List<IRule>(rules); 
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

        // Stand-in that just checks for empty cells for now
        public IEnumerable<Vector2Int> GetLegalPlacements(GameState game)
        {
            int n = game.Board.BoardSize;
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    if (game.Board.GetOccupant(x, y) == TileOccupant.None)
                        yield return new Vector2Int(x, y);
                }
            }
        }
    }
}