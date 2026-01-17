using UnityEngine;

namespace BP.Kingdoms.Core
{
    public abstract class Card : ICard
    {
        public abstract CardKey CardKey { get; }
        public int CardKeyId => (int)CardKey;
    }
}

