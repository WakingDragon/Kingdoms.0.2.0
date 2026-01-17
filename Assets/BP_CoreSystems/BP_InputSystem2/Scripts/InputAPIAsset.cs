using System;
using UnityEngine;
using UnityEngine.InputSystem;
using BP.Core;

namespace BP.InputSystem
{
    public enum ActionMapType { UI, DefaultPlayer }
    [CreateAssetMenu(fileName = "InputAPI", menuName = "Input/Input API Asset")]

    ///ADDING ACTION MAP
    ///Need to add interface for each new ActionMap e.g. BPInputActions.IUIActions
    ///Add to ActionMapType enum
    ///Need to implement the interface
    ///need to set callbacks for eah map
    ///create serializable events container class for each action map (see InputAPI_UIActionsEvents) - this makes the inspector tidier!
    ///add related events for each callback
    ///
    ///ADDING CONTROLS TO ACTION MAP
    ///[Need to add to Unity Action Map asset first and recompile the cs file]
    ///Ensure the new interface is added e.g. under UIAction / OnAnyKey
    ///Create the relevant SO event, and add this event to the relevant [mapName]ActionMapEvents] class
    ///add the SO event to the API asset
    
    public class InputAPIAsset : ScriptableObject, BPInputActions.IUIActions,BPInputActions.IDefaultPlayerActions
    {
        private BPInputActions _inputActions;
        [field:Header("Pause input")]
        [field:SerializeField] public bool isEnabled { get; private set; } = false;
        [SerializeField] private BoolGameEvent toggleEnableInputEvt;

        [SerializeField] private InputAPI_UIActionsEvents uiActionMapEvents;
        [SerializeField] private InputAPI_DefaultPlayerActions defaultPlayerActionMapEvents;
        [SerializeField] private ActionMapType _onStartActionMap = ActionMapType.UI;
        private ActionMapType _currentActionMap = ActionMapType.UI;

        public void OnEnable()
        {
            isEnabled = true;
            if(_inputActions == null) 
            { 
                _inputActions = new BPInputActions();
            }
            SetCallbacks();
            SetActionMap(_onStartActionMap);
        }

        #region UPDATE ACTION MAPS HERE
        private void SetCallbacks()
        {
            //set callbacks per action map
            _inputActions.UI.SetCallbacks(this);
            _inputActions.DefaultPlayer.SetCallbacks(this);
        }

        private void RemoveCallbacks()
        {
            //need to remove callbacks per action map
            if (_inputActions == null) return;
            _inputActions.UI.RemoveCallbacks(this);
            _inputActions.DefaultPlayer.RemoveCallbacks(this);
        }
        #endregion

        #region Action map switching
        public void SetActionMap(ActionMapType actionMapType)
        {
            switch (actionMapType)
            {
                case ActionMapType.UI:
                    SetUIActionMap();
                    break;
                case ActionMapType.DefaultPlayer:
                    SetDefaultPlayerActionMap();
                    break;
            }
            _currentActionMap = actionMapType;
        }

        private void SetUIActionMap()
        {
            _inputActions.UI.Enable();
            _inputActions.DefaultPlayer.Disable();
        }

        private void SetDefaultPlayerActionMap()
        {
            _inputActions.UI.Disable();
            _inputActions.DefaultPlayer.Enable();
        }
        #endregion

        private void OnDisable()
        {
            isEnabled = false;
            RemoveCallbacks();
            SetUIActionMap();
            _inputActions.UI.Disable();
            _inputActions.Disable();
            _inputActions = null;
        }

        #region UIActions
        public void OnAnyKey(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                //Debug.Log("anykey pressed:" + context.started);
                uiActionMapEvents.anyKeyEvent.Raise();
            }            
        }

        public void OnPauseUI(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                uiActionMapEvents.pauseEvt.Raise();
            }
        }
        #endregion 

        #region UIActions TBD
        public void OnPoint(InputAction.CallbackContext context)
        {
            //Debug.Log("Point:" + context.ReadValue<Vector2>());
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            //Debug.Log($"inputEnabled:{isEnabled},actionMapType:{_currentActionMap}");
            //Debug.Log("mouse clicked:" + context.performed);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }
        #endregion

        #region DefaultPlayerActions
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                defaultPlayerActionMapEvents.moveEvent.Raise(context.ReadValue<Vector2>());
            }
            else if (context.canceled)
            {
                defaultPlayerActionMapEvents.moveEvent.Raise(new Vector2());
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        { 
            if (context.started) defaultPlayerActionMapEvents.jumpEvent.Raise(true);
            if (context.canceled) defaultPlayerActionMapEvents.jumpEvent.Raise(false);
        }

        public void OnPausePlayer(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                uiActionMapEvents.pauseEvt.Raise();
            }
        }
        #endregion

        #region DefaultPlayerActions TBD

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
        }

        

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
        }
        #endregion
    }

    [System.Serializable]
    public class InputAPI_UIActionsEvents
    {
        [field:SerializeField] public VoidGameEvent anyKeyEvent { get; private set; }
        [field: SerializeField] public VoidGameEvent pauseEvt { get; private set; }
    }

    [System.Serializable]
    public class InputAPI_DefaultPlayerActions
    {
        [field: SerializeField] public Vector2GameEvent moveEvent { get; private set; }
        [field: SerializeField] public BoolGameEvent jumpEvent { get; private set; }
        [field: SerializeField] public VoidGameEvent pauseEvt { get; private set; }
    }
}

