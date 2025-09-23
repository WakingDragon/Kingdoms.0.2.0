using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    public sealed class CompositeRuleset : IRule
    {
        private readonly List<IRule> _rules;
        public CompositeRuleset(params IRule[] rules) { _rules = new List<IRule>(rules); }
        public RuleResult Evaluate(in GameState s, in IGameAction a)
        {
            foreach (var r in _rules)
            {
                var rResult = r.Evaluate(s, a);
                if (rResult.Outcome == RuleOutcome.Reject) return rResult; // short-circuit
            }
            return RuleResult.OK();
        }
    }
}