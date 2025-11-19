using UnityEngine;

namespace BP.Kingdoms.Core
{
    public static class CardFactory
    {
        public static ICard GetICardFromKey(CardKey key)
        {
            switch(key)
            {
                case CardKey.Null:
                    return new NullCard();
                //case CardKey.Fortify:
                //    return new FortifyCard();
                //case CardKey.Assassinate:
                //    return new AssassinateCard();
                //case CardKey.Trenches:
                //    return new TrenchesCard();
                //case CardKey.Border_Skirmish:
                //    return new BorderSkirmishCard();
                //case CardKey.Guerilla_Tactics:
                //    return new GuerillaTacticsCard();
                //case CardKey.Sabotage:
                //    return new SabotageCard();
                //case CardKey.Spies:
                //    return new SpiesCard();
                //case CardKey.Counter_Offensive:
                //    return new CounterOffensiveCard();
                //case CardKey.Supply_Lines:
                //    return new SupplyLinesCard();
                default:
                    Debug.LogError($"CardFactory: GetICardFromKey received unknown key {key}");
                    return null;
            }
        }
    }
}

