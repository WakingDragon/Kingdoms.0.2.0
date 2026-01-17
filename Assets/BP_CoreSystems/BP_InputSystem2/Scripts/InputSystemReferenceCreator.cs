using UnityEngine;

namespace BP.InputSystem
{
    public class InputSystemReferenceCreator : MonoBehaviour
    {
        [SerializeField] private InputAPIAsset _inputAsset;

        private void Awake()
        {
            //kicks starts the asset but making a reference to it.
            if (!_inputAsset.isEnabled) _inputAsset.OnEnable();
        }
    }
}

