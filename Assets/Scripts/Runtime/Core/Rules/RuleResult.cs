using System.Collections.Generic;

namespace BP.Kingdoms.Core
{
    public sealed class RuleResult
    {
        public string RuleName { get; set; }
        public RuleVerdict Verdict { get; private set; }
        public string Message { get; private set; }
        public List<RuleOutcome> Outcomes { get; } = new();

        public RuleResult(string ruleName, RuleVerdict verdict) 
        { 
            RuleName = ruleName;
            Verdict = verdict; 
        }

        public static RuleResult Success(string name, params RuleOutcome[] outcomes)
        {
            var ruleResult = new RuleResult(name, RuleVerdict.Success);
            if (outcomes != null) ruleResult.Outcomes.AddRange(outcomes);
            return ruleResult;
        }

        public static RuleResult Fail(string name) => new RuleResult(name, RuleVerdict.Fail);
        public static RuleResult NotApplicable(string name) => new RuleResult(name, RuleVerdict.NotApplicable);

    }
}