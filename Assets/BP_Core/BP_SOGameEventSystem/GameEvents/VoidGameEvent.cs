using UnityEngine;

namespace BP.Core
{
    [CreateAssetMenu(fileName ="new_voidEvt",menuName ="Core/Game Events/Void Game Event")]
    public class VoidGameEvent : BaseGameEvent<VoidType>
    {
        public void Raise()
        {
            Raise(new VoidType());
        }
    }
}


