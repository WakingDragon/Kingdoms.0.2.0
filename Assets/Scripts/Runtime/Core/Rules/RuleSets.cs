using BP.Kingdoms.Rules;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Kingdoms.Core
{
    public static class RuleSets
    {
        public static IRule Phase1() => new CompositeRuleSet(
            new MustBeOnBoard(),
            new MustBeActivePlayer(),
            new CellMustBeFree()
        // add your adjacency/flip rules here as you bring them online
        );
    }
}