namespace BP.Kingdoms.Core
{
    public sealed class RuleOutcome
    {
        public OutcomeKind Kind { get; }
        public int X { get; }
        public int Y { get; }
        public PlayerId FlipTo { get; } // only for Flip

        private RuleOutcome(OutcomeKind kind, int x, int y, PlayerId flipTo = PlayerId.P1)
        { Kind = kind; X = x; Y = y; FlipTo = flipTo; }

        public static RuleOutcome Placement(int x, int y) => new RuleOutcome(OutcomeKind.Placement, x, y);
        public static RuleOutcome Flip(int x, int y, PlayerId to) => new RuleOutcome(OutcomeKind.Flip, x, y, to);
        public static RuleOutcome CardEffect() => new RuleOutcome(OutcomeKind.CardEffect, -1, -1);
    }
}