namespace BP.Kingdoms.Core
{
    public sealed class RuleResult
    {
        public RuleOutcome Outcome { get; private set; }
        public string Message { get; private set; }
        private RuleResult(RuleOutcome o, string m = null) { Outcome = o; Message = m; }
        public static RuleResult OK() => new RuleResult(RuleOutcome.Ok);
        public static RuleResult Reject(string msg) => new RuleResult(RuleOutcome.Reject, msg);
    }
}