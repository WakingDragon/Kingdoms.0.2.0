using UnityEngine;

namespace BP.Kingdoms.Core
{
    public class NullCard : ICard
    {
        public CardKey CardKey => CardKey.Null;
        public int CardKeyId => (int)CardKey;
    }
}

